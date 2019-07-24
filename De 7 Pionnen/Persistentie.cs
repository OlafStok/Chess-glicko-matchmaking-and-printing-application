using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace De_7_Pionnen
{
    class Persistentie
    {
        public static void Opslaan(Object objectOmOpTeSlaan, string bestandsnaam)
        {
            Debug.WriteLine(bestandsnaam + " opgeslagen");
            Stream SaveFileStream = File.Create(bestandsnaam);
            BinaryFormatter serializer = new BinaryFormatter();
            serializer.Serialize(SaveFileStream, objectOmOpTeSlaan);
            SaveFileStream.Close();
        }

        public static Object Laad(string bestandsnaam)
        {
            Debug.WriteLine(bestandsnaam + " geladen");
            if (File.Exists(bestandsnaam))
            {
                try
                {
                    Console.WriteLine("Reading saved file");
                    Stream openFileStream = File.OpenRead(bestandsnaam);
                    BinaryFormatter deserializer = new BinaryFormatter();
                    var toreturn = deserializer.Deserialize(openFileStream);
                    openFileStream.Close();
                    return toreturn;
                }
                catch (Exception)
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
    }
}
