using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Unicode;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using VoterService.Models;
using VoterService.Utility;

namespace VoterService.Controllers
{
    public class VoterController : Controller
    {
        private readonly IConfiguration _configuration;
        private ApplicationDbContext _applicationDbContext;
        public VoterController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            return Ok("Voter Service");
        }

        // Check whether created database, if not create db * table using dbcontext according to district
        private bool PrepareDB(int nIdx)
        {
            // Set Database connection setting using district id
            Global.DefaultConnection = _configuration.GetConnectionString("DefaultConnection") + nIdx;
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseSqlServer(Global.DefaultConnection);

            _applicationDbContext = new ApplicationDbContext(optionsBuilder.Options);

            // Check whether db is created, if not create new db
            try
            {
                _applicationDbContext.Database.EnsureCreated();
            }
            catch
            {
                return false;
            }
            return true;
        }

        [HttpPost]
        public IActionResult CreateUpdateVoter([FromBody]VoterModel voter)
        {
            // Check there is table for district, if not exist create new DB & table
            bool IsDbReady = PrepareDB(voter.district);
            if (!IsDbReady)
            {
                return Ok(new Message<string>
                {
                    ReturnMessage = "Can't connect db",
                    IsSuccess = false
                });
            }

            //Add or Update new voter in database
            if (voter.id > 0)
            {
                _applicationDbContext.Voters.Update(voter);
            }
            else
            {
                _applicationDbContext.Voters.Add(voter);
            }
            _applicationDbContext.SaveChanges();

            //Generate Unique voter secret using id and nic
            string uniqueId = voter.id + "_" + voter.nic;
            uniqueId = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(uniqueId));
            uniqueId = uniqueId.Replace("=", "");
            voter.userid = uniqueId;
            _applicationDbContext.Voters.Update(voter);
            _applicationDbContext.SaveChanges();

            return Ok(new Message<string>
            {
                ReturnMessage = "Sucess",
                IsSuccess = true,
                Data = uniqueId
            });
        }

        //nIdx : district id
        [HttpGet]
        public IActionResult GetResult(int district)
        {
            // Check there is table for district, if not exist create new DB & table
            bool IsDbReady = PrepareDB(district);
            if (!IsDbReady)
            {
                return Ok(new Message<string>
                {
                    ReturnMessage = "Can't connect db",
                    IsSuccess = false
                });
            }

            VoteResult results = new VoteResult();

            results.vote = _applicationDbContext.Voters.Where(e => e.vote_state == Global.VOTED).Count();
            results.not_vote = _applicationDbContext.Voters.Where(e => e.vote_state == Global.ENTERED).Count();
            results.registerdVoter = _applicationDbContext.Voters.Count();

            return Ok(new Message<VoteResult>
            {
                ReturnMessage = "Sucess",
                IsSuccess = true,
                Data = results
            });
        }

        [HttpPost]
        public IActionResult GetVoter([FromBody]VoterSearchModel voterSearch)
        {
            if (voterSearch == null)
            {
                return Ok(new Message<VoterModel>
                {
                    ReturnMessage = "Fail",
                    IsSuccess = false
                });
            }
            VoterModel resultVoter = null;

            foreach(int nIdx in voterSearch.arrDistricts)
            {
                try
                {
                    PrepareDB(nIdx);
                    resultVoter = _applicationDbContext.Voters.Where(s => s.userid == voterSearch.userID).First();
                }
                catch
                {
                    continue;
                }

                // If found voter, return
                if (resultVoter != null)
                {
                    return Ok(new Message<VoterModel>
                    {
                        ReturnMessage = "Success",
                        IsSuccess = true,
                        Data = resultVoter
                    });
                }
            }

            return Ok(new Message<VoterModel>
            {
                ReturnMessage = "Fail",
                IsSuccess = false
            });
        }

        //get all voters in district
        [HttpGet]
        public IActionResult GetAllVoter(int district)
        {
            bool IsDbReady = PrepareDB(district);
            if (IsDbReady)
            {
                List<VoterModel> arrVoters = _applicationDbContext.Voters.ToList();
                return Ok(new Message<List<VoterModel>>
                {
                    ReturnMessage = "Success",
                    IsSuccess = true,
                    Data = arrVoters
                });
            }

            return Ok(new Message<List<VoterModel>>
            {
                ReturnMessage = "Fail",
                IsSuccess = false
            });
        }

        [HttpPost]
        public IActionResult Vote([FromBody]Vote_Model vote)
        {
            // Check there is table for district, if not exist create new DB & table
            bool IsDbReady = PrepareDB(vote.district);
            if (!IsDbReady)
            {
                return Ok(new Message<string>
                {
                    ReturnMessage = "Can't connect db",
                    IsSuccess = false
                });
            }
            try
            {
                VoterModel voter = _applicationDbContext.Voters.Where(e => e.userid == vote.userid).First();
                if (voter != null)
                {
                    voter.vote_state = vote.state;
                    _applicationDbContext.Voters.Update(voter);
                    _applicationDbContext.SaveChanges();
                    return Ok(new Message<int>
                    {
                        ReturnMessage = "Success",
                        IsSuccess = true
                    });
                }
            }
            catch
            { }

            return Ok(new Message<int>
            {
                ReturnMessage = "Fail",
                IsSuccess = false
            });
        }

        [HttpPost]
        public IActionResult DeleteVoter([FromBody]VoterSearchModel voterSearch)
        {
            if (voterSearch == null)
            {
                return Ok(new Message<int>
                {
                    ReturnMessage = "Fail",
                    IsSuccess = false
                });
            }
            try
            {
                PrepareDB(voterSearch.arrDistricts.First());
                VoterModel resultVoter = _applicationDbContext.Voters.Where(s => s.userid == voterSearch.userID).First();
                _applicationDbContext.Voters.Remove(resultVoter);
                _applicationDbContext.SaveChanges();
                return Ok(new Message<int>
                {
                    ReturnMessage = "Success",
                    IsSuccess = true
                });
            }
            catch { }

            return Ok(new Message<int>
            {
                ReturnMessage = "Fail",
                IsSuccess = false
            });
        }
    }
}