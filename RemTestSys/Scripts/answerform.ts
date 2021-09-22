interface IAnswerForm {
    htmlElement: HTMLElement;
    form: HTMLElement;
    showAndGetAnswer(): Promise<Answer>;
    fill(additive: any);
    hide();
    generateForm(arg:any);
}

class ConfirmForm implements IAnswerForm {
    htmlElement: HTMLElement;
    form: HTMLElement;
    constructor(textOfBtn: string) {
        this.generateForm(textOfBtn);
    }
    generateForm(arg:any) {
        this.htmlElement = document.createElement("div");
        this.htmlElement.classList.add("hidden");
        this.form = document.createElement("form");
        let submitBtn = document.createElement("input");
        submitBtn.type = "submit";
        submitBtn.value = arg;
        this.form.append(submitBtn);
        this.htmlElement.append(this.form);
    }
    showAndGetAnswer(): Promise<Answer> {
        let answer = new Answer();
        this.htmlElement.classList.remove("hidden");
        return new Promise<Answer>(
            (resolve, reject) => {
                this.form.onsubmit = () => {
                    resolve(answer);
                    return false;
                }
            }
        );
    }
    fill(additive: any) {}
    hide() {
        this.htmlElement.classList.add("hidden");
    }
}

class TextAnswerForm implements IAnswerForm{
    htmlElement: HTMLElement;
    form: HTMLElement;
    input: HTMLInputElement;
    constructor(textOfBtn: string, placeholder: string) {
        this.generateForm({ textOfBtn, placeholder });
    }
    generateForm(arg: any) {
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
    showAndGetAnswer(): Promise<Answer> {
        let answer = new Answer();
        this.htmlElement.classList.remove("hidden");
        return new Promise<Answer>((resolve, reject) => {
            this.form.onsubmit = () => {
                answer.type = "text";
                answer.data = this.input.value;
                return false;
            };
        });
    }
    fill(additive: any) {}
    hide() {
        this.htmlElement.classList.add("hidden");
    }
}