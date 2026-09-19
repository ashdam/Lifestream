using Dalamud.Game.Text.SeStringHandling.Payloads;
using Dalamud.Utility;
using ECommons.ExcelServices.Sheets;
using Lumina.Text.ReadOnly;

namespace Lifestream;

internal static class GameText
{
    internal static string Dialogue(string sheet, uint row) =>
        Svc.Data.GetExcelSheet<QuestDialogueText>(name: sheet).GetRow(row).Value.GetText().Trim();

    // Match a literal segment without joining text across variable parameters.
    internal static string[] Fragments(params ReadOnlySeString[] rows) => rows
        .Select(row => row.ToDalamudString().Payloads.OfType<TextPayload>()
            .Select(payload => payload.Text?.Trim())
            .Where(text => text != null && text.Any(char.IsLetter))
            .OrderByDescending(text => text.Length)
            .FirstOrDefault())
        .Where(text => !string.IsNullOrEmpty(text))
        .ToArray();
}
