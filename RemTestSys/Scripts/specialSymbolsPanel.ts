function registerSpecialSymbolsPanel(input: HTMLInputElement) {
    let switcher = document.querySelector("#specialSymbolsPanel button[data-state]") as HTMLButtonElement;
    let inptSymbols = document.querySelectorAll("#specialSymbolsPanel button[data-symbol]") as NodeListOf<HTMLButtonElement>;

    if (!input) throw new ReferenceError("input isn't passed");
    if (!switcher) throw new ReferenceError("switcher isn't found");
    if (!inptSymbols) throw new ReferenceError("inptSymbols isn't found");

    switcher.click = () => {
        if (switcher.dataset.state == "on") {
            switcher.dataset.state = "off";
        } else {
            switcher.dataset.state = "on";
        }
        input.focus();
    };
    for (let btn of inptSymbols) {
        btn.click = () => {
            input.value = input.value + btn.dataset.symbol;
            input.focus();
        };
    }
    input.onkeydown = () => {

    };
}