using Expense.Logger.Business.Interfaces;
using Expense.Logger.Business.Models.Transaction;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Expense.Logger.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class TransactionsController : ControllerBase
{
    public ITransactionsHandler _transactionsHandler;

    public TransactionsController(ITransactionsHandler transactionsHandler) : base()
    {
        _transactionsHandler = transactionsHandler;
    }

    [HttpPost]
    public ActionResult Create([FromBody][Required] TransactionCreate create)
    {
        return Ok(_transactionsHandler.CreateAsync(create));
    }

    [HttpGet("{id}")]
    public ActionResult GetById(long id)
    {
        return Ok(_transactionsHandler.GetByIdAsync(id));
    }

    [HttpGet]
    public ActionResult Get([FromQuery] TransactionQuery query)
    {
        return Ok(_transactionsHandler.GetByPageAsync(query));
    }
}
