using CRM.API.ViewModels;
using CRM.Data.Entities;
using CRM.Data.Enums;
using CRM.Service.Helpers;
using CRM.Service.Interfaces.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CRM.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class JobTransactionsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public JobTransactionsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: api/Transaction
        [HttpGet]
        public async Task<IActionResult> GetTransactions()
        {
            try
            {

                var transactions = await _unitOfWork.JobTransactionsRepository.GetAllAsync();

                if (transactions == null || !transactions.Any())
                {
                    return NotFound("No transactions found.");
                }

                var transactionsViewModel = transactions.Select(transaction => new JobTransactionsViewModel
                {
                    Id = transaction.Id,
                    JobId = transaction.JobId,
                    TransactionNumber = transaction.Number,
                    TransactionDescription = transaction.Description,
                    TransactionStatus = transaction.Status,
                    TransactionComment = transaction.Comment,
                    Timestamp = transaction.Timestamp
                }).ToList();

                return Ok(transactionsViewModel);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error loading transactions: " + ex.Message);
                return StatusCode(500, "Internal Server Error");
            }
        }

        // GET: api/Transaction/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTransaction(int id)
        {
            var transaction = await _unitOfWork.JobTransactionsRepository.GetByIdAsync(c => EF.Property<int>(c, "Id") == id);
            if (transaction == null)
            {
                return NotFound($"Transaction with ID {id} not found.");
            }

            var transactionViewModel = new JobTransactionsViewModel
            {
                Id = transaction.Id,
                JobId = transaction.JobId,
                TransactionNumber = transaction.Number,
                TransactionDescription = transaction.Description,
                TransactionStatus = transaction.Status,
                TransactionComment = transaction.Comment,
                Timestamp = transaction.Timestamp
            };

            return Ok(transactionViewModel);
        }

        // GET: api/Transaction/job/{jobId}
        [HttpGet("job/{jobId}")]
        public async Task<IActionResult> GetTransactionsByJobId(int jobId)
        {
            try
            {
                var transactions = await _unitOfWork.JobTransactionsRepository.GetAllAsync(filter: t => t.JobId == jobId);


                if (transactions == null || !transactions.Any())
                {
                    return NotFound($"No transactions found for JobId {jobId}");
                }


                var transactionsViewModel = transactions.Select(transaction => new JobTransactionsViewModel
                {
                    Id = transaction.Id,
                    JobId = transaction.JobId,
                    TransactionNumber = transaction.Number,
                    TransactionDescription = transaction.Description,
                    TransactionStatus = transaction.Status,
                    TransactionComment = transaction.Comment,
                    Timestamp = transaction.Timestamp
                }).ToList();

                return Ok(transactionsViewModel);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error loading transactions: " + ex.Message);
                return StatusCode(500, "Internal Server Error");
            }
        }

        // POST: api/Transaction
        [HttpPost]
        public async Task<IActionResult> CreateTransaction(JobTransactionsViewModel transactionModel)
        {
            try
            {
                var transaction = new JobTransactions
                {
                    JobId = transactionModel.JobId,
                    Number = transactionModel.TransactionNumber,
                    Description = transactionModel.TransactionDescription,
                    Status = transactionModel.TransactionStatus,
                    Comment = transactionModel.TransactionComment,
                    Timestamp = transactionModel.Timestamp
                };

                var createdTransaction = await _unitOfWork.JobTransactionsRepository.AddAsync(transaction);
                await _unitOfWork.SaveChangesAsync();

                return CreatedAtAction("GetTransaction", new { id = createdTransaction.Id }, createdTransaction);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error creating transaction: " + ex.Message);
                return StatusCode(500, "Internal Server Error");
            }
        }

        // PUT: api/Transaction/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTransaction(int id, JobTransactionsViewModel transactionModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != transactionModel.Id)
            {
                return BadRequest();
            }

            try
            {
                var existingTransaction = await _unitOfWork.JobTransactionsRepository.GetByIdAsync(c => EF.Property<int>(c, "Id") == id);
                if (existingTransaction == null)
                {
                    return NotFound();
                }

                existingTransaction.Number = transactionModel.TransactionNumber;
                existingTransaction.Description = transactionModel.TransactionDescription;
                existingTransaction.Status = transactionModel.TransactionStatus;
                existingTransaction.Comment = transactionModel.TransactionComment;
                existingTransaction.Timestamp = transactionModel.Timestamp;

                var updatedTransaction = await _unitOfWork.JobTransactionsRepository.UpdateAsync(existingTransaction);
                await _unitOfWork.SaveChangesAsync();

                return Ok(updatedTransaction);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error updating transaction: " + ex.Message);
                return StatusCode(500, "Internal Server Error");
            }
        }

        // DELETE: api/Transaction/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransaction(int id)
        {
            try
            {
                var deleted = await _unitOfWork.JobTransactionsRepository.DeleteAsync(id.ToString());


                if (!deleted)
                {
                    return NotFound();
                }

                await _unitOfWork.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error deleting transaction: " + ex.Message);
                return StatusCode(500, "Internal Server Error");
            }
        }



        // POST: api/Transaction/CreateJobTransactions
        [HttpPost("CreateJobTransactions")]
        [AllowAnonymous]
        public async Task<IActionResult> CreateJobTransactions(CreateJobTransaction jobTransactionModel)  
        {
            try
            {
                var jobTransactions = await _unitOfWork.JobTransactionsRepository.GetAllAsync();

                var existingJobTransactions = jobTransactions.FirstOrDefault(f => f.JobId == jobTransactionModel.JobId);

                if (existingJobTransactions == null)
                {
                    var objJobTransaction = new JobTransactions
                    {
                        JobId = jobTransactionModel.JobId,
                        Description = jobTransactionModel.TransactionDescription,
                        Number = jobTransactionModel.TransactionNumber,
                        Comment = jobTransactionModel.TransactionCommand,
                        Timestamp = jobTransactionModel.Timestamp,
                        Status = jobTransactionModel.TransactionStatus
                    };

                    await _unitOfWork.JobTransactionsRepository.AddAsync(objJobTransaction);
                    await _unitOfWork.SaveChangesAsync();
                    return Ok("Job transaction created successfully");
                }

                else
                {
                    existingJobTransactions.Description = jobTransactionModel.TransactionDescription;
                    existingJobTransactions.Number = jobTransactionModel.TransactionNumber;
                    existingJobTransactions.Comment = jobTransactionModel.TransactionCommand;
                    existingJobTransactions.Status = jobTransactionModel.TransactionStatus;
                    await _unitOfWork.JobTransactionsRepository.UpdateAsync(existingJobTransactions);
                    await _unitOfWork.SaveChangesAsync();

                    return Ok("Job Transaction updated successfully");
                }

            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }


    }
}
