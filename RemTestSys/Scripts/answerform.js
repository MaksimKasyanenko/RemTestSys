class ConfirmForm {
    constructor() {
        this.htmlElement = document.getElementById("confirmFormWrp");
        this.form = document.querySelector("#confirmFormWrp form");
        if (!this.htmlElement || !this.form) {
            throw new ReferenceError("confirmForm can't be built, not all of required elements was found");
        }
    }
    showAndGetAnswer() {
        let answer = new Answer();
        this.htmlElement.classList.remove("hidden");
        return new Promise((resolve, reject) => {
            this.form.onsubmit = e => {
                e.preventDefault();
                resolve(answer);
            };
        });
    }
    fill(additive) { }
    hide() {
        this.htmlElement.classList.add("hidden");
    }
}
class TextAnswerForm {
    constructor() {
        this.htmlElement = document.getElementById("textAnswerFormWrp");
        this.form = document.querySelector("#textAnswerFormWrp form");
        this.input = document.querySelector("#textAnswerFormWrp form input[type='text']");
        if (!this.htmlElement || !this.form || !this.input) {
            throw new ReferenceError("textForm can't be built, not all of required elements was found");
        }
    }
    showAndGetAnswer() {
        let answer = new Answer();
        this.htmlElement.classList.remove("hidden");
        return new Promise((resolve, reject) => {
            this.form.onsubmit = e => {
                e.preventDefault();
                answer.data = [this.input.value];
                this.input.value = "";
                resolve(answer);
            };
        });
    }
    fill(additive) {
        this.input.value = "";
    }
    hide() {
        this.htmlElement.classList.add("hidden");
    }
}
class OneOfFourVariantsAnswerForm {
    constructor() {
        this.htmlElement = document.getElementById("OneVariantAnswerFormWrp");
        this.form = document.querySelector("#OneVariantAnswerFormWrp form");
        this.input = document.querySelector("#OneVariantAnswerFormWrp input[type='hidden']");
        let btns = document.querySelectorAll("#OneVariantAnswerFormWrp button");
        this.buttons = Array.from(btns);
        if (!this.htmlElement || !this.form || !this.input || !this.buttons) {
            throw new ReferenceError("oneVariantForm can't be built, not all of required elements was found");
        }
        this.initButtons();
    }
    showAndGetAnswer() {
        let answer = new Answer();
        this.htmlElement.classList.remove("hidden");
        return new Promise((resolve, reject) => {
            this.form.onsubmit = e => {
                e.preventDefault();
                answer.data = [this.input.value];
                this.input.value = "";
                resolve(answer);
            };
        });
    }
    fill(additive) {
        for (let i = 0; i < this.buttons.length; i++) {
            this.buttons[i].classList.remove("choosed");
            this.buttons[i].textContent = additive[i];
        }
    }
    hide() {
        this.htmlElement.classList.add("hidden");
    }
    initButtons() {
        let clearChoosed = () => {
            for (let btn of this.buttons) {
                btn.classList.remove("choosed");
            }
        };
        for (let btn of this.buttons) {
            btn.onclick = e => {
                clearChoosed();
                this.input.value = btn.textContent;
                btn.classList.add("choosed");
            };
        }
    }
}
