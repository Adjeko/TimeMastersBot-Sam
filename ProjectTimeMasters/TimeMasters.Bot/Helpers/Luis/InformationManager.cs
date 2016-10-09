using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Management;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;

namespace TimeMasters.Bot.Helpers.Luis
{
    public class InformationManager<T>
    {
        public List<T> Forms { get; set; }

        public InformationManager()
        {
            Forms = new List<T>();
            Forms.Add((T) Activator.CreateInstance(typeof(T)));
        }

        public void ProcessResult(LuisResult result)
        {
            EntityRecommendation primary = FindPrimaryEntity(result.Entities);
            if (primary != null)
            {
                foreach (EntityRecommendation er in result.Entities)
                {
                    ProcessEntity(primary, er);
                }
            }
            else
            {
                //make a output, because primary value was not found in this qeue.
            }
            
        }

        private void ProcessEntity(EntityRecommendation primary, EntityRecommendation entity)
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
                    entityDateTime = new Chronic.Parser().Parse(entity.Entity).ToTime();
                }


                bool shouldBreak = false;
                foreach (T c in Forms)
                {

                    bool primBool = (string)primaryProperty.GetValue(c) == primary.Entity || primaryProperty.GetValue(c) == null;

                    //check if value has already been set in the current Form
                    if (p.GetValue(c) == null && primBool)
                    {
                        //set the value
                        p.SetValue(c, Convert.ChangeType(entity.Entity, p.PropertyType), null);
                        shouldBreak = true;
                    }
                    if (p.PropertyType == typeof(DateTime) && ((DateTime)p.GetValue(c)).Equals(new DateTime()) && primBool)
                    {
                        //set the value
                        p.SetValue(c, Convert.ChangeType(entityDateTime, p.PropertyType), null);
                        shouldBreak = true;
                    }
                    //value already set -> check the next Form
                }

                if (shouldBreak) continue;

                //no form with an unset property was found
                //create new form
                Forms.Add((T)Activator.CreateInstance(typeof(T)));
                //set primary value of newly created form with the entity found in result
                primaryProperty.SetValue(Forms.Last(), Convert.ChangeType(primary.Entity, primaryProperty.PropertyType), null);
                //and set itself in the new form
                if (p.PropertyType == typeof(DateTime))
                {
                    p.SetValue(Forms.Last(), Convert.ChangeType(entityDateTime, p.PropertyType), null);
                }
                else
                {
                    p.SetValue(Forms.Last(), Convert.ChangeType(entity.Entity, p.PropertyType), null);
                }
                



                /*foreach (object o in attrs)
                {
                    

                    LuisIdentifierAttribute tmp = o as LuisIdentifierAttribute;
                    if (tmp?.Value != entity.Type) continue;
                   
                    if (p.PropertyType == typeof(DateTime))
                    {
                        DateTime time = new DateTime();
                        time = new Chronic.Parser().Parse(entity.Entity).ToTime();
                        p.SetValue(Forms[0], Convert.ChangeType(time, p.PropertyType), null);
                    }
                    else
                    {
                        p.SetValue(Forms[0], Convert.ChangeType(entity.Entity, p.PropertyType), null);
                    }
                }*/
            }
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
                        if (luis != null)
                        {
                            return entities.Where(e => e.Type == luis.Value).ElementAt(0);
                        }
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

        public string GetMissingInformation()
        {
            return "";
        }
    }
}