using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using Microsoft.Win32;

namespace JecPizzaTestUI
{
    public class EmailTemplateBulderHelper
    {
        private const string TemplatesDirectory = @"C:\Study\School\JecPizza\JecPizza\Content\MailTemplates\";
        public object TargetObject { get; set; }



        public EmailTemplateBulderHelper(object TargetObject) { this.TargetObject = TargetObject; }

        public static bool SaveNewTemplateAsJson(Window owner, string data)
        {
            SaveFileDialog sfd = new SaveFileDialog()
            {
                Title = "New Email Template",
                FileName = "EmailTemplateSettings",
                AddExtension = true,
                DefaultExt = "json",
                InitialDirectory = TemplatesDirectory

            };

            if ((bool)sfd.ShowDialog(owner).Value)
            {
                using var sw = new StreamWriter(sfd.OpenFile(), Encoding.UTF8);
                sw.Write(data);
                return true;
            }

            return false;
        }

        public static List<string> GetProperties(Type t) => t.GetProperties().Select(p => p.Name).ToList();

        public static object GetPropValue(object src, string propName) => src.GetType().GetProperty(propName)?.GetValue(src, null);
    }
}