using System.Collections.Generic;

namespace Laberinto.CORE
{
    public interface IWarehouseService
    {
        public List<Cell> GenerateWarehouse(List<Cell> warehouse);
        public List<Cell> SetExitCellFormat(List<Cell> warehouse);
        public List<Cell> RestoreWarehouseFromFile(string filename = "warehouse.json");
    }
}