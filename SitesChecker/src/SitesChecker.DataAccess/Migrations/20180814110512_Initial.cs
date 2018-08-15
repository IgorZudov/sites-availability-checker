using Microsoft.EntityFrameworkCore.Migrations;

namespace SitesChecker.DataAccess.Migrations
{
	public partial class Initial : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				name: "SiteAvailabilities",
				columns: table => new
				{
					Id = table.Column<int>(nullable: false)
						.Annotation("Sqlite:Autoincrement", true),
					Name = table.Column<string>(nullable: true),
					Url = table.Column<string>(nullable: true)
				},
				constraints: table => { table.PrimaryKey("PK_SiteAvailabilities", x => x.Id); });
			migrationBuilder.CreateTable(
				name: "Users",
				columns: table => new
				{
					Id = table.Column<int>(nullable: false)
						.Annotation("Sqlite:Autoincrement", true),
					Login = table.Column<string>(nullable: true),
					Password = table.Column<string>(nullable: true),
					Role = table.Column<string>(nullable: true)
				},
				constraints: table => { table.PrimaryKey("PK_Users", x => x.Id); });
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "SiteAvailabilities");
			migrationBuilder.DropTable(
				name: "Users");
		}
	}
}