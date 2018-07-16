using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Reflection;
using System.Collections;
using System.Net.Http;

namespace CondorExtreme3Console
{
    public class VectorN
    {
       
        private List<float> Coordinates { get; set; }

        public int NumberOfCoordinates
        {
            get { return Coordinates.Count; }
        }


      

        public VectorN(float[] Coordinates)
        {
            this.Coordinates = new List<float>();
            foreach (var item in Coordinates)
            {
                this.Coordinates.Add(item);
            }
            
        }
      
        public static float DotProduct(VectorN v1, VectorN v2)
        {
            if (v1.NumberOfCoordinates != v2.NumberOfCoordinates)
                throw new Exception("Vectors need to have same length!");
            float dt = 0;
            for (int i = 0; i < v1.NumberOfCoordinates; i++)
                dt += (v1.Coordinates[i] * v2.Coordinates[i]);        
            return dt;
        }
        public float GetLength()
        {
            float temp = 0;
            foreach (var item in this.Coordinates)
                temp += (float) Math.Pow(item, 2);
            return (float)(Math.Sqrt(temp));
        }

        public static float CosineSimilarity(VectorN v1, VectorN v2)
        {
            return DotProduct(v1, v2) / (v1.GetLength() * v2.GetLength());
        }
        public static VectorN Subtraction(VectorN v1, VectorN v2)
        {
            if (v1.NumberOfCoordinates != v2.NumberOfCoordinates)
                throw new Exception("Vectors need to have same length!");
            float[] ResultCord = new float[v1.NumberOfCoordinates];
            for (int i = 0; i < v1.NumberOfCoordinates; i++)
                ResultCord[i] = (v1.Coordinates[i] - v2.Coordinates[i]);
            return new VectorN(ResultCord);
        }
    }
    class Program
    {
        static void Main(string[] args)
        {

            WebAPIHelper Service = new WebAPIHelper(WebAPIHelper.ApiUri, "api/RecommendationSystem");

            HttpResponseMessage httpResponseMessage = Service.GetResponse("GetRecommendedMovies", 12);
            if (httpResponseMessage.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var Result = httpResponseMessage.Content.ReadAsAsync<Dictionary<int, float>>().Result;
                Console.WriteLine("Success");

            }
            else
            {
                Console.WriteLine("Error");
            }



        }
    }
}
