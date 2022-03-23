using Microsoft.EntityFrameworkCore.Migrations;
using Shared;

namespace FullFraim.Data.Migrations
{
    public partial class AddTriggerForRanks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(Constants.DatabaseQueries.RankTrigger);

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(Constants.DatabaseQueries.DropRankTrigger);
        }
    }
}
