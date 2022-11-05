// See https://aka.ms/new-console-template for more information
using Newtonsoft.Json;
using StorageModifier.Models;

Console.WriteLine("Storage Modifier!");

List<PatientStorage> InputStorage = new List<PatientStorage>();

List<PatientStorage> OutputStorage = new List<PatientStorage>();

string pathInput = Environment.CurrentDirectory + Path.DirectorySeparatorChar + "Input";

string [] input = Directory.GetFiles(pathInput);

string pathOutput = Environment.CurrentDirectory + Path.DirectorySeparatorChar + "Output";

string json = File.ReadAllText(pathInput);

InputStorage = (List<PatientStorage>)JsonConvert.DeserializeObject<List<PatientStorage>>(json);

Console.ReadKey();
