using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceAPI
{
    public class interfaceAPIyms : APIinterface
    {
        string uri;
        public HttpClient client;
        public interfaceAPIyms()
        {
            uri= "https://localhost:5243";
            client = new HttpClient();
        }
        public async Task<ExamsList> GetAllExams()
        {
         return await client.GetFromJsonAsync<ExamsList>(uri+ "/api/Insert/citySelector");
        }
        public async Task<int> InsertAExam(Exams exams)
        {
            return (await client.PostAsJsonAsync<Exams>(uri + "/api/Insert/InsertAExam", exams)).IsSuccessStatusCode ? 1 : 0;
        }
        public async Task<int> UpdateAExam(Exams exams)
        {
            return (await client.PutAsJsonAsync<Exams>(uri + "/api/Insert/UpdateAExam", exams)).IsSuccessStatusCode ? 1 : 0;
        }
        public async Task<int> DeleteAExam(int id)
        {
            return (await client.DeleteAsync(uri + "/api/Insert/DeleteAExam/" + id)).IsSuccessStatusCode ? 1 : 0;
        }
    }
}
