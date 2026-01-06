using Expense.Logger.Business.Interfaces;
using Expense.Logger.Business.Models.Transaction;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Expense.Logger.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class TransactionsController(ITransactionsHandler transactionsHandler) : ControllerBase()
{
    public ITransactionsHandler _transactionsHandler = transactionsHandler;

    [HttpPost]
    public async Task<ActionResult> Create([FromBody][Required] TransactionCreate create) =>
        Ok(await _transactionsHandler.CreateAsync(create));

    [HttpGet("{id}")]
    public async Task<ActionResult> GetById(long id) => Ok(await _transactionsHandler.GetByIdAsync(id));


    [HttpGet]
    public async Task<ActionResult> Get([FromQuery] TransactionQuery query) => Ok(await _transactionsHandler.GetByPageAsync(query));
}
