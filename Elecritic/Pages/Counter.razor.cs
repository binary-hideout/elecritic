namespace Elecritic.Pages {
    public partial class Counter {

        private int CurrentCount { get; set; } = 0;

        private void IncrementCount() {
            CurrentCount++;
        }
    }
}