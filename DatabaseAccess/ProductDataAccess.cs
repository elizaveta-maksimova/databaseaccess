using System;
using DatabaseAccess.Abstract;

namespace DatabaseAccess
{
    public class ProductDataAccess : AbstractDatabaseAccess<Product>
    {
        public override string TableName => "Product";

        public override string ConnectionString => @"Server=DESKTOP-7PGPI07;Integrated Security=true;Initial Catalog=TestDatabase";
    }

    public class Product : DatabaseEntity
    {
        [DataColumn("id", true, false)]
        public int Id { get; set; }

        [DataColumn("name")]
        public string Name { get; set; }

        [DataColumn("description")]
        public string Description { get; set; }

        [DataColumn("update_date")]
        public DateTime? UpdateDate { get; set; }
    }

    public class MiniProduct : DatabaseEntity
    {
        [DataColumn("id", true, false)]
        public int Id { get; set; }

        [DataColumn("name")]
        public string Name { get; set; }
    }

    public class MiniInsertProduct : DatabaseEntity
    {
        [DataColumn("name")]
        public string Name { get; set; }


        [DataColumn("description")]
        public string Description { get; set; }
    }

    public class MiniUpdateProduct : MiniInsertProduct
    {
        [DataColumn("id", true, false)]
        public int Id { get; set; }
    }
}
