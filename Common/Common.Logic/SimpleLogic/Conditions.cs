using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Common.Logic.XML.Interfaces;

namespace Common.Logic.XML.SimpleLogic
{
    [Serializable]
    public class Conditions : CheckCollectionBase<ICondition>, IConditions
    {

         public bool Check(string conditionName, IDictionary<string, string> circs)
         {
             foreach (var condition in this)
             {
                 if (condition.Name == conditionName)return condition.Check(circs);
             }
             throw new Exception(string.Format("В Conditions не найдено не одного Condition с именем {0}.", conditionName));
         }

        public void Serialize(string pathToDoc)
        {
            Serialize().Save(pathToDoc);
        }

        public static IConditions Deserialize(string pathToDoc)
        {
            var xDoc = XDocument.Load(pathToDoc);
            var consditions = Constants.Factory.Deserialize(xDoc.Root) as IConditions;
           if(consditions==null)
               throw new Exception("Заглавный элемент XML не имеет имя Conditions.");

            return consditions;  
        }

        
        public override bool Check(IDictionary<string, string> circs)
        {
            if(Count!=1)
                throw new Exception("Не возможно произвести Check() с одним аргументом. Количество элементов Condition не равно 1");
            return this.First().Check(circs);
        }

        public override IEnumerable<ICheckable> ChildCheckers
        {
            get { return this; }
        }

        public override string ItemName { get { return Constants.Conditions; } }
    }
}