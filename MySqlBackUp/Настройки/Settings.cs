using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace MySqlBackUp
{
    public class Settings
    {
        /// <summary>
        /// Имя файла
        /// </summary>
        private readonly string fileName = "settings.xml";
        //кодировка
        System.Text.Encoding encoding = System.Text.Encoding.UTF8;

        private readonly string tagPath = "Path";
        private readonly string tagCount = "CountDoBackUp";

        /// <summary>
        /// Путь к каталогу
        /// </summary>
        public string Path { get; set; }
        /// <summary>
        /// Количество бэкапов в сутки
        /// </summary>
        public int CountDoBackUp { get; set; }

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
                    {
                        Path = node.InnerText;
                    }

                    if (node.Name == tagCount)
                    {
                        CountDoBackUp = int.Parse(node.InnerText);
                    }
                }
            }
            catch
            {
                Path = "";
                CountDoBackUp = 1;
            }
        }
    }
}
