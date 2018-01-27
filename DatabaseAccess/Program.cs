using System;

namespace DatabaseAccess
{
    class Program
    {
        static void Main(string[] args)
        {
            TestDatabase();
        }

        private static void TestDatabase()
        {
            ProductDataAccess pda = new ProductDataAccess();
            
            Product p = new Product
            {
                Description = "Some definely good staff",
                Name = "Important staff",
                UpdateDate = DateTime.UtcNow
            };
            
            pda.Insert(p);
            //var result = pda.GetRecords<MiniProduct>();

            //var result2 = pda.GetRecord<MiniProduct>(product => product.Id == 2);

            //MiniInsertProduct p = new MiniInsertProduct
            //{
            //    Description = "This is very good good",
            //    Name = "Some more staff",
            //};

            //pda.Insert(p);

            //MiniUpdateProduct up = new MiniUpdateProduct();
            //up.Id = 4;
            //up.Description = "It is anither product";
            //up.Name = "Another product";

            //pda.Update(up);

            //result2.Name = "Chopper2";

            //pda.Update(result2);
        }
    }
}
