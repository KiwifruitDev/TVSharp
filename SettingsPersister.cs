using System;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace TVSharp
{
    // Token: 0x0200000B RID: 11
    public class SettingsPersister
    {
        // Token: 0x06000059 RID: 89 RVA: 0x000027B4 File Offset: 0x000009B4
        public SettingsPersister()
        {
            this._settingsFolder = Path.GetDirectoryName(Application.ExecutablePath);
        }

        // Token: 0x0600005A RID: 90 RVA: 0x000027CC File Offset: 0x000009CC
        public SettingsMemoryEntry ReadSettings()
        {
            SettingsMemoryEntry settingsMemoryEntry = this.ReadObject<SettingsMemoryEntry>("settings.xml");
            if (settingsMemoryEntry != null)
            {
                return settingsMemoryEntry;
            }
            return new SettingsMemoryEntry();
        }

        // Token: 0x0600005B RID: 91 RVA: 0x000027EF File Offset: 0x000009EF
        public void PersistSettings(SettingsMemoryEntry entries)
        {
            this.WriteObject<SettingsMemoryEntry>(entries, "settings.xml");
        }

        // Token: 0x0600005C RID: 92 RVA: 0x00002800 File Offset: 0x00000A00
        private T ReadObject<T>(string fileName)
        {
            string path = Path.Combine(this._settingsFolder, fileName);
            if (File.Exists(path))
            {
                using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
                    return (T)((object)xmlSerializer.Deserialize(fileStream));
                }
            }
            return default(T);
        }

        // Token: 0x0600005D RID: 93 RVA: 0x00002874 File Offset: 0x00000A74
        private void WriteObject<T>(T obj, string fileName)
        {
            string path = Path.Combine(this._settingsFolder, fileName);
            using (FileStream fileStream = new FileStream(path, FileMode.Create))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(obj.GetType());
                xmlSerializer.Serialize(fileStream, obj);
            }
        }

        // Token: 0x04000023 RID: 35
        private const string FreqMgrFilename = "settings.xml";

        // Token: 0x04000024 RID: 36
        private readonly string _settingsFolder;
    }
}
