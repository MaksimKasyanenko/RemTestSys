class ConfirmForm {
    constructor(textOfBtn) {
        this.generateForm(textOfBtn);
    }
    generateForm(arg) {
        this.htmlElement = document.createElement("div");
        this.htmlElement.classList.add("hidden");
        this.form = document.createElement("form");
        let submitBtn = document.createElement("input");
        submitBtn.type = "submit";
        submitBtn.value = arg;
        this.form.append(submitBtn);
        this.htmlElement.append(this.form);
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
    constructor(textOfBtn, placeholder) {
        this.generateForm({ textOfBtn, placeholder });
    }
    generateForm(arg) {
        this.htmlElement = document.createElement("div");
        this.htmlElement.classList.add("hidden");
        this.form = document.createElement("form");
        let sbmt = document.createElement("input");
        sbmt.type = "submit";
        sbmt.value = arg.textOfBtn;
        this.input = document.createElement("input");
        this.input.placeholder = arg.placeholder;
        this.form.append(this.input);
        this.form.append(sbmt);
        let row = document.createElement("div");
        row.append(this.form);
        this.htmlElement.append(row);
    }
    showAndGetAnswer() {
        let answer = new Answer();
        this.htmlElement.classList.remove("hidden");
        return new Promise((resolve, reject) => {
            this.form.onsubmit = e => {
                e.preventDefault();
                answer.data = this.input.value;
                resolve(answer);
            };
        });
    }
    fill(additive) { }
    hide() {
        this.htmlElement.classList.add("hidden");
    }
}
