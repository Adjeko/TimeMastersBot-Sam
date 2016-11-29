using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using System.Web.Management;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using TimeMasters.Bot.Helpers.Luis.Calendar;
using TimeMasters.Bot.Helpers.Luis.DateTimeParser;

namespace TimeMasters.Bot.Helpers.Luis
{
    [Serializable]
    public class InformationManager<T> where T : ILuisForm
    {
        private string _debugMessage;

        public List<T> Forms { get; set; }

        public InformationManager()
        {
            Forms = new List<T> {(T) Activator.CreateInstance(typeof(T))};
        }

        public void ProcessResult(LuisResult result)
        {
            _debugMessage += $"\"{result.Query}\"\n\n";

            //first sort the entities list primary > required > unrequired
            IList<EntityRecommendation> sortedEntities = SortEntityList(result.Entities);

            // ???
            //foreach (EntityRecommendation e in sortedEntities)
            //{
            //    e.Entity = new string(e.Entity.Where(c => !char.IsWhiteSpace(c)).ToArray());
            //}

            _debugMessage += "Input sorted EntityList: \n\n\n\n";
            foreach(var e in sortedEntities)
            {
                _debugMessage += $"{e.Type} -> {e.Entity}\n\n";
            }

            EntityRecommendation primary = FindPrimaryEntity(sortedEntities);
            if (primary != null)
            {
                _debugMessage += $"\n\nPrimary Entity is : {primary.Entity}\n\n\n\n";
                foreach (EntityRecommendation er in sortedEntities)
                {
                    ProcessEntityWithPrimary(result, primary, er);
                }
            }
            else
            {
                foreach (EntityRecommendation er in sortedEntities)
                {
                    ProcessEntityWithoutPrimary(result, er);
                }
            }

            _debugMessage += "Result of Processing Input is: \n\n";
            foreach(T t in Forms)
            {
                _debugMessage += t.ToString() + "\n\n";
            }

            foreach (T t in Forms)
            {
                t.TryResolveMissingInformation();
            }

            _debugMessage += "Result of Resolving Missing Information is: \n\n";
            foreach (T t in Forms)
            {
                _debugMessage += t.ToString() + "\n\n";
            }
        }

        private void ProcessEntityWithoutPrimary(LuisResult result, EntityRecommendation entity)
        {
            //assume that this entity addresses a missing required property in the first Form
            foreach (PropertyInfo p in typeof(T).GetProperties())
            {
                object[] attrs = p.GetCustomAttributes(false);
                if (!PropertyContainsLuisIdentifier(attrs, entity.Type)) continue;

                DateTime entityDateTime = new DateTime();

                if (p.PropertyType == typeof(DateTime))
                {
                    string luisBuildtinText = "";
                    //find BuiltinIdentifier
                    foreach(object o in attrs)
                    {
                        LuisBuiltInIdentifierAttribute lb = o as LuisBuiltInIdentifierAttribute;
                        if(lb != null)
                        {
                            luisBuildtinText = lb.Value;
                        }
                    }

                    DateTime tmp = DateTimeParser.DateTimeParser.ParseDateTime(result, entity, luisBuildtinText, out string tmpDebug);
                    if (!tmp.Equals(new DateTime()))
                    {
                        entityDateTime = tmp;
                        _debugMessage += tmpDebug;
                    }
                    else
                    {
                        _debugMessage += tmpDebug;
                        _debugMessage += $"{entity.Entity} could not be parsed\n\n";
                        return;
                    }
                }

                //found the corresponding property
                if (p.GetValue(Forms[0]) == null)
                {
                    p.SetValue(Forms[0], Convert.ChangeType(entity.Entity, p.PropertyType), null);
                }
                if (p.PropertyType == typeof(DateTime) && ((DateTime) p.GetValue(Forms[0])).Equals(new DateTime()))
                {
                    p.SetValue(Forms[0], Convert.ChangeType(entityDateTime, p.PropertyType), null);
                }

            }

        }

        private void ProcessEntityWithPrimary(LuisResult result, EntityRecommendation primary, EntityRecommendation entity)
        {
            Type t = typeof(T);
            PropertyInfo[] props = t.GetProperties();

            PropertyInfo primaryProperty = FindPrimaryPropery(props);

            
            foreach (PropertyInfo p in props)
            {
                object[] attrs = p.GetCustomAttributes(false);

                //check if this Property can handle the current Luis Entity
                if (!PropertyContainsLuisIdentifier(attrs, entity.Type)) continue;

                DateTime entityDateTime = new DateTime();

                if (p.PropertyType == typeof(DateTime))
                {
                    string luisBuildtinText = "";
                    //find BuiltinIdentifier
                    foreach (object o in attrs)
                    {
                        LuisBuiltInIdentifierAttribute lb = o as LuisBuiltInIdentifierAttribute;
                        if (lb != null)
                        {
                            luisBuildtinText = lb.Value;
                        }
                    }

                    DateTime tmp = DateTimeParser.DateTimeParser.ParseDateTime(result, entity, luisBuildtinText, out string tmpDebug);
                    if (!tmp.Equals(new DateTime()))
                    {
                        entityDateTime = tmp;
                        _debugMessage += tmpDebug;
                    }
                    else
                    {
                        _debugMessage += tmpDebug;
                        _debugMessage += $"{entity.Entity} could not be parsed\n\n";
                        return;
                    }
                }


                bool shouldBreak = false;
                foreach (T c in Forms)
                {
                    bool primBool = (string) primaryProperty.GetValue(c) == primary.Entity ||
                                    primaryProperty.GetValue(c) == null;

                    //check if value has already been set in the current Form
                    if (p.GetValue(c) == null && primBool)
                    {
                        //set the value
                        p.SetValue(c, Convert.ChangeType(entity.Entity, p.PropertyType), null);
                        shouldBreak = true;
                    }
                    if (p.PropertyType == typeof(DateTime) && ((DateTime) p.GetValue(c)).Equals(new DateTime()) &&
                        primBool)
                    {
                        //set the value
                        p.SetValue(c, Convert.ChangeType(entityDateTime, p.PropertyType), null);
                        shouldBreak = true;
                    }
                    //value already set -> check the next Form
                }

                if (shouldBreak) continue;

                //build new object by copying an existing one
                T newT = (T) Activator.CreateInstance(typeof(T));
                PropertyInfo[] newProps = typeof(T).GetProperties();
                foreach (PropertyInfo info in newProps)
                {
                    //make a copy of the first element in Forms
                    var value = info.GetValue(Forms.ElementAt(0));
                    info.SetValue(newT, value);
                }

                //no form with an unset property was found
                //create new form
                Forms.Add(newT);
                //set primary value of newly created form with the entity found in result
                //primaryProperty.SetValue(Forms.Last(), Convert.ChangeType(primary.Entity, primaryProperty.PropertyType),
                //    null);
                //and set itself in the new form
                p.SetValue(Forms.Last(),
                    p.PropertyType == typeof(DateTime)
                        ? Convert.ChangeType(entityDateTime, p.PropertyType)
                        : Convert.ChangeType(entity.Entity, p.PropertyType), null);
                
            }
        }


        private IList<EntityRecommendation> SortEntityList(IList<EntityRecommendation> entities)
        {
            List<EntityRecommendation> tmp1 = new List<EntityRecommendation>(entities);
            List<EntityRecommendation> tmp2 = new List<EntityRecommendation>(entities);

            List<EntityRecommendation> result = new List<EntityRecommendation>();
            
            //first find primary
            foreach (EntityRecommendation e in tmp1)
            {
                if (IsLuisIdentifierPrimary(e.Type))
                {
                    result.Add(e);
                    tmp2.Remove(e);
                }
            }
            tmp1 = new List<EntityRecommendation>(tmp2);
            //then find all required
            foreach (EntityRecommendation e in tmp1)
            {
                if (IsLuisIdentifierRequired(e.Type))
                {
                    result.Add(e);
                    tmp2.Remove(e);
                }
            }
            tmp1 = new List<EntityRecommendation>(tmp2);
            //last add all unrequired
            foreach (EntityRecommendation e in tmp1)
            {
                result.Add(e);
                tmp2.Remove(e);
            }
            
            return result;
        }

        private bool IsLuisIdentifierPrimary(string luisIdentifier)
        {
            PropertyInfo primProp = FindPrimaryPropery(typeof(T).GetProperties());

            object[] attrs = primProp.GetCustomAttributes(false);

            if (PropertyContainsLuisIdentifier(attrs, luisIdentifier))
            {
                //search for isPrimary Attribute
                foreach (object o in attrs)
                {
                    IsPrimaryAttribute a = o as IsPrimaryAttribute;
                    if (a != null)
                    {
                        return a.Value;
                    }
                }
            }
            return false;
        }

        private bool IsLuisIdentifierRequired(string luisIdentifier)
        {
            Type type = typeof(T);
            PropertyInfo[] props = type.GetProperties();

            bool isRightProperty = false;
            bool isRequiredProperty = false;

            foreach (PropertyInfo p in props)
            {
                object[] attrs = p.GetCustomAttributes(false);
                foreach (object o in attrs)
                {
                    LuisIdentifierAttribute lia = o as LuisIdentifierAttribute;
                    if (lia != null && lia.Value == luisIdentifier)
                    {
                        isRightProperty = true;
                    }
                    IsRequiredAttribute ira = o as IsRequiredAttribute;
                    if (ira != null)
                    {
                        isRequiredProperty = ira.Value;
                    }

                    if (isRightProperty && isRequiredProperty)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private EntityRecommendation FindPrimaryEntity(IList<EntityRecommendation> entities)
        {
            Type t = typeof(T);
            PropertyInfo[] props = t.GetProperties();
            foreach (PropertyInfo p in props)
            {
                if (PropertyIsPrimary(p))
                {
                    //this property is the primary one
                    //now read the LuisIdentifier and find it in the entities list
                    foreach (object attrs in p.GetCustomAttributes(false))
                    {
                        LuisIdentifierAttribute luis = attrs as LuisIdentifierAttribute;
                        if (luis == null) continue;
                        var list = entities.Where(e => e.Type == luis.Value);
                        return list.Any() ? list.ElementAt(0) : null;
                    }
                }
            }
            return null;
        }

        private PropertyInfo FindPrimaryPropery(PropertyInfo[] props)
        {
            foreach (PropertyInfo p in props)
            {
                if (PropertyIsPrimary(p)) return p;
            }
            return null;
        }

        private bool PropertyIsPrimary(PropertyInfo property)
        {
            object[] attributes = property.GetCustomAttributes(false);

            foreach (object o in attributes)
            {
                IsPrimaryAttribute prim = o as IsPrimaryAttribute;
                if (prim != null)
                {
                    return prim.Value;
                }
            }
            return false;
        }

        private bool PropertyContainsLuisIdentifier(object[] attributes, string identifier)
        {
            foreach (object o in attributes)
            {
                LuisIdentifierAttribute luis = o as LuisIdentifierAttribute;
                if (luis != null)
                {
                    return luis.Value == identifier;
                }
            }
            return false;
        }

        private PropertyInfo[] FormHasMissingRequiredProperties(T form)
        {
            Type t = form.GetType();
            PropertyInfo[] props = t.GetProperties();

            //get all properties where IsRequiredAttribute is true
            var result = props.Where((e) =>
            {
                object[] attr = e.GetCustomAttributes(false);
                var required = attr.Where(a => (a as IsRequiredAttribute) != null);

                if (e.PropertyType == typeof(DateTime))
                {
                    return ((IsRequiredAttribute) required.ElementAt(0)).Value &&
                           ((DateTime) e.GetValue(form)).Equals(new DateTime());
                }
                return ((IsRequiredAttribute) required.ElementAt(0)).Value && (string) e.GetValue(form) == null;
            });

            return result.ToArray();
        }

        private PropertyInfo[] GetAllSetRequiredProperties(T form)
        {
            Type t = form.GetType();
            PropertyInfo[] props = t.GetProperties();

            var result = props.Where((e) =>
            {
                object[] attr = e.GetCustomAttributes(false);
                var required = attr.Where(a => (a as IsRequiredAttribute) != null);

                if (e.PropertyType == typeof(DateTime))
                {
                    return ((IsRequiredAttribute) required.ElementAt(0)).Value &&
                           !((DateTime) e.GetValue(form)).Equals(new DateTime());
                }
                return ((IsRequiredAttribute) required.ElementAt(0)).Value && (string) e.GetValue(form) != null;
            });

            return result.ToArray();
        }

        private bool IsFormFilledWithRequiredProperties(T form)
        {
            return FormHasMissingRequiredProperties(form).Length == 0;
        }

        public string GetNextMissingInformation()
        {
            string result = "I need ";
            foreach (T t in Forms)
            {
                var tmp = FormHasMissingRequiredProperties(t);
                if (tmp == null || tmp.Length == 0) continue;

                foreach (PropertyInfo p in tmp)
                {
                    result += $" {p.Name} ";
                }
                result += "to finish ";

                var fin = GetAllSetRequiredProperties(t);
                foreach (PropertyInfo p in fin)
                {
                    result += $" {p.Name}:{p.GetValue(t)}  ";
                }

                break;
            }

            return result;
        }

        public List<T> GetFinishedEntries()
        {
            List<T> finishedEntries = new List<T>();

            //find finished entries in Forms
            for (int i = 0; i < Forms.Count; i++)
            {
                T tmp = Forms.ElementAt(i);
                if (IsFormFilledWithRequiredProperties(tmp))
                {
                    finishedEntries.Add(tmp);
                }
            }

            //delete finished entries from Forms
            foreach (T t in finishedEntries)
            {
                Forms.Remove(t);
            }

            return finishedEntries;
        }

        public bool IsInformationRequired()
        {
            foreach (T t in Forms)
            {
                if (!IsFormFilledWithRequiredProperties(t))
                {
                    return true;
                }
            }
            return false;
        }

        public void Clear()
        {
            Forms = new List<T> { (T)Activator.CreateInstance(typeof(T)) };
        }

        public string GetDebugMessage()
        {
            return _debugMessage;
        }
    }
}