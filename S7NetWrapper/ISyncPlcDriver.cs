#region Using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; 
#endregion

namespace S7NetWrapper
{
    public interface IPlcSyncDriver
    {        
        ConnectionStates ConnectionState { get; }
        
        void Connect();

        void Disconnect();        

        List<Tag> ReadItems(List<Tag> itemList);

        void ReadClass(object sourceClass, int db);

        void WriteClass(object sourceClass, int db);      
       
        void WriteItems(List<Tag> itemList); 
    }   
}
