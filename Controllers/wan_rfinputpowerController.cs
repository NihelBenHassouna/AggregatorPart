using AggregatorPart.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace AggregatorPart.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class wan_rfinputpowerController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        public wan_rfinputpowerController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public JsonResult Get()
        {
            string query = @"
                select 
                    d_id as ""d_id"",
                    d_maxrxlevel as ""d_maxrxlevel""
                from wan_rfinputpower
            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("conenctionPgsql");
            NpgsqlDataReader myReader;
            using (NpgsqlConnection myCon = new NpgsqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (NpgsqlCommand myCommand = new NpgsqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    myCon.Close();

                }
            }

            return new JsonResult(table);
        }

       
    }
}
