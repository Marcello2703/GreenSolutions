namespace GreenSolutions.DTOs.BudgetDTOs
{
    public class CreateBudgetDTO
    {
        public int UserId { get; set; }
        public int ClientId { get; set; }

        public int CompanyId { get; set; }

        public List<BudgetItemDTO> Items { get; set; }

    }
}
