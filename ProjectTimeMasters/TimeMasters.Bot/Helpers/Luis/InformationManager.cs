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
            foreach (EntityRecommendation er in result.Entities)
            {
                ProcessEntity(er);
            }
        }

        private void ProcessEntity(EntityRecommendation entity)
        {
            Type t = typeof(Calendar);
            PropertyInfo[] props = t.GetProperties();
            foreach (PropertyInfo p in props)
            {
                object[] attrs = p.GetCustomAttributes(false);

                //check if current property is primary
                //adjust number of forms
                if (CheckIsPrimary(attrs) && CheckContainsLuisIdentifier(attrs, entity.Entity))
                {
                    //check if a form with this primary value already exists
                    Forms.Count(e => (string)p.GetValue(e) == entity.Entity);
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

        private bool CheckIsPrimary(object[] attributes)
        {
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

        private bool CheckContainsLuisIdentifier(object[] attributes, string identifier)
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