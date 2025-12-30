using Expense.Logger.Business.Interfaces;
using Expense.Logger.Business.Models;
using Expense.Logger.Business.Models.Transactions;
using Microsoft.AspNetCore.Mvc;

namespace Expense.Logger.Server.Controllers;

[ApiController]
[Route("[controller]")]
public class TransactionsController : ControllerBase
{
    public ITransactionsHandler _transactionsHandler { get; set; }

    public TransactionsController(ITransactionsHandler transactionsHandler) : base()
    {
        _transactionsHandler = transactionsHandler;
    }

    [HttpGet("{id}")]
    public ActionResult GetById(long id)
    {
        return Ok(_transactionsHandler.GetByIdAsync(id));
    }

    [HttpGet]
    public ActionResult Get([FromQuery] TransactionRequestQuery query)
    {
        return Ok(_transactionsHandler.GetByPageAsync(query));
    }
}
