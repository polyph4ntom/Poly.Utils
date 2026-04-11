namespace Poly.Serialization
{
    public sealed class FPolyIniWriteOptions
    {
        public string NewLine { get; set; } = "\n";
        public bool BlankLineBetweenSections { get; set; } = true;
        public bool OmitFinalTrailingNewLine { get; set; } = false;
    }
}