﻿function registerSpecialSymbolsPanel(input: HTMLInputElement) {
    let inptSymbols = document.querySelectorAll("#specialSymbolsPanel button") as NodeListOf<HTMLButtonElement>;

    if (!input) throw new ReferenceError("input isn't passed");
    if (!inptSymbols) throw new ReferenceError("inptSymbols isn't found");

    for (let btn of inptSymbols) {
        btn.onclick = () => {
            input.value = input.value + btn.dataset.symbol;
            input.focus();
        };
    }
}