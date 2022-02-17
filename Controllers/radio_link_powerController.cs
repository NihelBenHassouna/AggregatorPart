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
    public class radio_link_powerController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        public radio_link_powerController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public JsonResult Get()
        {
            string query = @"
                select 
                    c_id as ""c_id"",
                    c_maxrxlevel as ""c_maxrxlevel""
                from radio_link_power
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
