using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRMS_Core.Migrations
{
    /// <inheritdoc />
    public partial class AddIsGeofencingRequired : Migration
    {
        /// <inheritdoc />
        // dotnet ef migrations add also picked up District (CompanyDetails),
        // Remark (AttendanceRegularization), AttendanceLimit (AspNetUsers) and
        // AccessLevel (AspNetRoles) here — this project's migration history
        // and the live DB have drifted (those columns already exist in the
        // DB; a prior migration adding some of them was authored but never
        // recorded as applied). Trimmed Up()/Down() down to just the one
        // column that's genuinely missing so this migration is safe to run;
        // the Designer.cs/snapshot still reflect the full current model.
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsGeofencingRequired",
                table: "AspNetUsers",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsGeofencingRequired",
                table: "AspNetUsers");
        }
    }
}
