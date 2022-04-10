using Laberinto.API.Helpers.Extensions;
using Laberinto.CORE;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace Laberinto.API.Controllers
{
    [ApiController]
    [EnableCors("MyPolicy")]
    [Route("[controller]")]
    public class CellController : ControllerBase
    {
        private readonly IWarehouseService _warehouseService;

        public CellController(IWarehouseService warehouseService)
        {
            _warehouseService = warehouseService;
        }

        [HttpGet]
        public IEnumerable<Cell> Get()
        {
            List<Cell> warehouse = new List<Cell>();

            if (System.IO.File.Exists("warehouse.json"))
            {
                warehouse = _warehouseService.RestoreWarehouseFromFile();
                warehouse = _warehouseService.SetExitCellFormat(warehouse);
            }

            return warehouse.ToArray();
        }

        [HttpGet("CalculateDistances")]
        public IEnumerable<Cell> CalculateDistances()
        {
            List<Cell> warehouse = new List<Cell>();

            if (System.IO.File.Exists("warehouse.json"))
            {
                warehouse = _warehouseService.RestoreWarehouseFromFile();
                warehouse = _warehouseService.GenerateWarehouse(warehouse);
            }

            return warehouse.ToArray();
        }
    }
}

public class WarehouseService : IWarehouseService
{
    public List<Cell> Warehouse { get; set; }

    public List<Cell> GenerateWarehouse(List<Cell> warehouse)
    {
        this.Warehouse = warehouse;
        Cell exitCell = new Cell() { X = 0, Y = 0, Value = 0, Hall = true };
        CheckCell(exitCell);
        SetExitCellFormat(this.Warehouse);

        return this.Warehouse;
    }

    private void CheckCell(Cell cell)
    {
        if (cell.X - 1 >= 0)
        {
            Cell leftCell = Warehouse.FirstOrDefault(leftCell => leftCell.X == cell.X - 1 && leftCell.Y == cell.Y);
            if (leftCell != null && leftCell.Hall && (leftCell.Value == 0 || leftCell.Value > cell.Value + 1))
            {
                leftCell.Value = cell.Value + 1;
                CheckCell(leftCell);
            }
        }
        if (cell.Y - 1 >= 0)
        {
            Cell bottomCell = Warehouse.FirstOrDefault(bottomCell => bottomCell.X == cell.X && bottomCell.Y == cell.Y - 1);
            if (bottomCell != null && bottomCell.Hall && (bottomCell.Value == 0 || bottomCell.Value > cell.Value + 1))
            {
                bottomCell.Value = cell.Value + 1;
                CheckCell(bottomCell);
            }
        }
        if (cell.X + 1 >= 0)
        {
            Cell rightCell = Warehouse.FirstOrDefault(rightCell => rightCell.X == cell.X + 1 && rightCell.Y == cell.Y);
            if (rightCell != null && rightCell.Hall && (rightCell.Value == 0 || rightCell.Value > cell.Value + 1))
            {
                rightCell.Value = cell.Value + 1;
                CheckCell(rightCell);
            }
        }
        if (cell.Y + 1 >= 0)
        {
            Cell topCell = Warehouse.FirstOrDefault(topCell => topCell.X == cell.X && topCell.Y == cell.Y + 1);
            if (topCell != null && topCell.Hall && (topCell.Value == 0 || topCell.Value > cell.Value + 1))
            {
                topCell.Value = cell.Value + 1;
                CheckCell(topCell);
            }
        }
    }

    public List<Cell> SetExitCellFormat(List<Cell> warehouse)
    {
        warehouse.First(x => x.X == 0 && x.Y == 0).Value = 0;
        return warehouse; 
    }

    public List<Cell> RestoreWarehouseFromFile(string filename = "warehouse.json") => System.IO.File.ReadAllText(filename).FromJson<List<Cell>>();
}