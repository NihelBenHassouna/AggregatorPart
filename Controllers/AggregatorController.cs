using AggregatorPart.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace AggregatorPart.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AggregatorController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public AggregatorController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public JsonResult Get()
        {
            
            string query = @"
                select 
                    a_id as ""a_id"",
                    a_date as ""a_date"",
                    a_rsl_deviation as ""a_rsl_deviation""
                from aggregator
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

        [HttpPost]
        public JsonResult Post(Aggregator dep)
        { 
            string query = @"
            INSERT into aggregator (a_date,a_rsl_deviation) 
            VALUES(CURRENT_TIMESTAMP, @a_rsl_deviation)
            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("conenctionPgsql");
            NpgsqlDataReader myReader;
            using (NpgsqlConnection myCon = new NpgsqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (NpgsqlCommand myCommand = new NpgsqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@a_rsl_deviation", dep.a_rsl_deviation);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    myCon.Close();

                }
            }

            return new JsonResult("Added Successfully");
        }
    }
}
