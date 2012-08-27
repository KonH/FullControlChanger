using System;
using System.Collections.Generic;
using System.Text;

using System.IO;
using System.Security.AccessControl;

namespace FullControlChanger
{
    class FullControlChanger
    {
        /// <summary>
        /// User identity
        /// </summary>
        string identity;

        /// <summary>
        /// List of changed files
        /// </summary>
        public List<string> changed;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="identity">Identity of user (like DOMAIN\user_name)</param>
        public FullControlChanger(string identity)
        {
            this.identity = identity;
            changed = new List<string>();
        }

        /// <summary>
        /// Change privilegies on "filename" to Full Control.
        /// Recour method - process all files (and directories) in "filename" 
        /// if it is directory.
        /// </summary>
        public void Change(string filename)
        {
            if (Directory.Exists(filename))
            {
                string[] dirs = Directory.GetDirectories(filename);
                foreach (string dir in dirs)
                {
                    Change(dir);
                }

                string[] files = Directory.GetFiles(filename);
                foreach (string file in files)
                {
                    Change(file);
                }
            }
            else
            {
                if (File.Exists(filename))
                {
                    FileSecurity sec = File.GetAccessControl(filename);
                    AuthorizationRuleCollection rules = sec.GetAccessRules(true, true, typeof(System.Security.Principal.NTAccount));

                    foreach (FileSystemAccessRule arule in rules)
                    {
                        if (arule.IdentityReference.Value == identity)
                        {
                            if (arule.FileSystemRights == FileSystemRights.FullControl)
                            {
                                // If FullControl to user already exists
                                return;
                            }
                        }
                    }

                    FileSystemAccessRule rule = new FileSystemAccessRule(identity, FileSystemRights.FullControl, AccessControlType.Allow);
                    sec.AddAccessRule(rule);
                    File.SetAccessControl(filename, sec);

                    changed.Add(filename);
                }
            }
        }
    }
}
