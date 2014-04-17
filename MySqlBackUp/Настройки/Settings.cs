using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace MysqlBackup
{
    public class Settings
    {
        /// <summary>
        /// Имя файла
        /// </summary>
        private readonly string fileName = System.Windows.Forms.Application.StartupPath + "\\settings.xml";
        //кодировка
        private Encoding encoding = Encoding.UTF8;
        private readonly string tagPath = "Path";
        private readonly string tagCount = "CountDoBackUp";
        private readonly string tagIsDeleteOldFiles = "IsDeleteOldFiles";
        private readonly string tagIsAutorun = "IsAutorun";

        /// <summary>
        /// Путь к каталогу
        /// </summary>
        public string Path { get; set; }
        /// <summary>
        /// Количество бэкапов в сутки
        /// </summary>
        public int CountDoBackUp { get; set; }
        /// <summary>
        /// Удалить старые файлы
        /// </summary>
        public bool IsDeleteOldFiles { get; set; }
        /// <summary>
        /// Автозапуск при входе в систему
        /// </summary>
        public bool IsAutorun {get;set;}

        /// <summary>
        /// Сохранить в файл
        /// </summary>
        public void SaveToFile()
        {
            var xmlDocument = new XmlDocument();

            //Описание xml
            var xmlDeclaration = xmlDocument.CreateXmlDeclaration("1.0", encoding.HeaderName, null);
            xmlDocument.InsertBefore(xmlDeclaration, xmlDocument.DocumentElement);

            //создаю основной узел
            XmlElement rootNode = xmlDocument.CreateElement("root");

            var node = xmlDocument.CreateElement(tagPath);
            node.InnerText = Path;
            rootNode.AppendChild(node);

            node = xmlDocument.CreateElement(tagCount);
            node.InnerText = CountDoBackUp.ToString();
            rootNode.AppendChild(node);

            node = xmlDocument.CreateElement(tagIsDeleteOldFiles);
            node.InnerText = IsDeleteOldFiles.ToString();
            rootNode.AppendChild(node);

            node = xmlDocument.CreateElement(tagIsAutorun);
            node.InnerText = IsAutorun.ToString();
            rootNode.AppendChild(node);

            //Добавляю заголовок в файл xml
            xmlDocument.AppendChild(rootNode);

            xmlDocument.Save(fileName);
        }

        /// <summary>
        /// Загрузить из файла
        /// </summary>
        public void LoadFromFile()
        {
            var xmlDocument = new XmlDocument();
            try
            {
                xmlDocument.Load(fileName);

                foreach (XmlNode node in xmlDocument.DocumentElement.ChildNodes)
                {
                    if (node.Name == tagPath)
                        Path = node.InnerText;

                    if (node.Name == tagCount)
                        CountDoBackUp = int.Parse(node.InnerText);

                    if (node.Name == tagIsDeleteOldFiles)
                        IsDeleteOldFiles = bool.Parse(node.InnerText);

                    if (node.Name == tagIsAutorun)
                        IsAutorun = bool.Parse(node.InnerText);
                }
            }
            catch
            {
                Path = "";
                CountDoBackUp = 1;
                IsDeleteOldFiles = true;
                IsAutorun = true;
            }
        }
    }
}
