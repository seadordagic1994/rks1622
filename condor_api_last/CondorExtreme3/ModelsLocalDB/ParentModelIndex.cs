using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CondorExtreme3.ModelsLocalDB;
using CondorExtreme3.DAL;
using CondorExtreme3.Helper;

namespace CondorExtreme3.ModelsLocalDB
{

    public class ParentModelIndex
    {
        public IndexVM IndexVM { get; set; }
        public Projection Projection { get; set; } = new Projection();
    }
}