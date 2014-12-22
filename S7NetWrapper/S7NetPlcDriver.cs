using S7.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace S7NetWrapper
{
    public class S7NetPlcDriver : IPlcSyncDriver
    {
        #region Private fields

        Plc client;     

        #endregion     

        #region Constructor

        public S7NetPlcDriver(CpuType cpu, string ip, short rack, short slot)
        {
            client = new Plc(cpu, ip, rack, slot);
        } 

        #endregion

        #region IPlcSyncDriver members

        private ConnectionStates _connectionState;
        public ConnectionStates ConnectionState
        {
            get { return _connectionState; }
            private set { _connectionState = value; }
        }

        public void Connect()
        {
            ConnectionState = ConnectionStates.Connecting;
            var error = client.Open();
            if (error != ErrorCode.NoError)
            {
                ConnectionState = ConnectionStates.Offline;
                throw new Exception(error.ToString());
            }
            ConnectionState = ConnectionStates.Online;
        }

        public void Disconnect()
        {
            ConnectionState = ConnectionStates.Offline;
            client.Close();
        }        

        public List<Tag> ReadItems(List<Tag> itemList)
        {
            if (this.ConnectionState != ConnectionStates.Online)
            {
                throw new Exception("Can't read, the client is disconnected.");
            }

            List<Tag> tags = new List<Tag>();
            foreach (var item in itemList)
            {
                Tag tag = new Tag(item.ItemName);
                var result = client.Read(item.ItemName);
                if (result is ErrorCode && (ErrorCode)result != ErrorCode.NoError)
                {
                    throw new Exception(((ErrorCode)result).ToString() + "\n" + "Tag: " + tag.ItemName);
                }
                tag.ItemValue = result;
                tags.Add(tag);
            }
            return tags;
        }

        public void WriteItems(List<Tag> itemList)
        {
            if (this.ConnectionState != ConnectionStates.Online)
            {
                throw new Exception("Can't write, the client is disconnected.");
            }

            foreach (var tag in itemList)
            {
                object value = tag.ItemValue;
                if (tag.ItemValue is double)
                {
                    var bytes = S7.Net.Types.Double.ToByteArray((double)tag.ItemValue);
                    value = S7.Net.Types.DWord.FromByteArray(bytes);
                }
                else if (tag.ItemValue is bool)
                {
                    value = (bool)tag.ItemValue ? 1 : 0;
                }
                var result = client.Write(tag.ItemName, value);
                if (result is ErrorCode && (ErrorCode)result != ErrorCode.NoError)
                {
                    throw new Exception(((ErrorCode)result).ToString() + "\n" + "Tag: " + tag.ItemName);
                }
            }
        }

        public void ReadClass(object sourceClass, int db)
        {
            client.ReadClass(sourceClass, db);
        }

        public void WriteClass(object sourceClass, int db)
        {
            client.WriteClass(sourceClass, db);
        }       

        #endregion
    }
}
