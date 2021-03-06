interface IAnswerForm {
    htmlElement: HTMLElement;
    form: HTMLFormElement;
    showAndGetAnswer(): Promise<Answer>;
    fill(additive: string[]);
    hide();
}

class ConfirmForm implements IAnswerForm {
    htmlElement: HTMLElement;
    form: HTMLFormElement;
    constructor() {
        this.htmlElement = document.getElementById("confirmFormWrp");
        this.form = document.querySelector("#confirmFormWrp form");
        if (!this.htmlElement || !this.form) {
            throw new ReferenceError("confirmForm can't be built, not all of required elements was found");
        }
    }
    showAndGetAnswer(): Promise<Answer> {
        let answer = new Answer();
        this.htmlElement.classList.remove("hidden");
        return new Promise<Answer>(
            (resolve, reject) => {
                this.form.onsubmit = e => {
                    e.preventDefault();
                    resolve(answer);
                }
            }
        );
    }
    fill(additive: string[]) {}
    hide() {
        this.htmlElement.classList.add("hidden");
    }
}

class TextAnswerForm implements IAnswerForm{
    htmlElement: HTMLElement;
    form: HTMLFormElement;
    input: HTMLInputElement;
    constructor() {
        this.htmlElement = document.getElementById("textAnswerFormWrp");
        this.form = document.querySelector("#textAnswerFormWrp form");
        this.input = document.querySelector("#textAnswerFormWrp form input[type='text']");
        if (!this.htmlElement || !this.form || !this.input) {
            throw new ReferenceError("textForm can't be built, not all of required elements was found");
        }
    }
    showAndGetAnswer(): Promise<Answer> {
        let answer = new Answer();
        this.htmlElement.classList.remove("hidden");
        return new Promise<Answer>((resolve, reject) => {
            this.form.onsubmit = e => {
                e.preventDefault();
                answer.data = [this.input.value];
                this.input.value = "";
                resolve(answer);
            };
        });
    }
    fill(additive: string[]) {
        this.input.value = "";
    }
    hide() {
        this.htmlElement.classList.add("hidden");
    }
}

class OneOfFourVariantsAnswerForm implements IAnswerForm {
    htmlElement: HTMLElement;
    form: HTMLFormElement;
    input: HTMLInputElement;
    buttons: HTMLButtonElement[];
    constructor() {
        this.htmlElement = document.getElementById("OneVariantAnswerFormWrp");
        this.form = document.querySelector("#OneVariantAnswerFormWrp form");
        this.input = document.querySelector("#OneVariantAnswerFormWrp input[type='hidden']");
        let btns = document.querySelectorAll<HTMLButtonElement>("#OneVariantAnswerFormWrp button");
        this.buttons = Array.from(btns);
        if (!this.htmlElement || !this.form || !this.input || !this.buttons) {
            throw new ReferenceError("oneVariantForm can't be built, not all of required elements was found");
        }
        this.initButtons();
    }
    showAndGetAnswer(): Promise<Answer> {
        let answer = new Answer();
        this.htmlElement.classList.remove("hidden");
        return new Promise<Answer>((resolve, reject) => {
            this.form.onsubmit = e => {
                e.preventDefault();
                answer.data = [this.input.value];
                this.input.value = "";
                resolve(answer);
            };
        });
    }
    fill(additive: string[]) {
        for (let i = 0; i < this.buttons.length; i++) {
            this.buttons[i].classList.remove("choosed");
            this.buttons[i].textContent = additive[i];
        }
    }
    hide() {
        this.htmlElement.classList.add("hidden");
    }
    private initButtons() {
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

class SomeVariantsAnswerForm implements IAnswerForm {
    htmlElement: HTMLElement;
    form: HTMLFormElement;
    list: HTMLUListElement;
    constructor() {
        this.htmlElement = document.getElementById("someVariantAnswerFormWrp");
        this.form = document.querySelector("#someVariantAnswerFormWrp form");
        this.list = document.querySelector("#someVariantAnswerFormWrp ul");
        if (!this.htmlElement || !this.form || !this.list) {
            throw new ReferenceError("someVariantForm can't be built, not all of required elements was found");
        }
    }
    showAndGetAnswer(): Promise<Answer> {
        let answer = new Answer();
        answer.data = [];
        this.htmlElement.classList.remove("hidden");
        return new Promise<Answer>((resolve, reject) => {
            this.form.onsubmit = e => {
                e.preventDefault();
                for (let ch of document.querySelectorAll("#someVariantAnswerFormWrp ul li [type='checkbox']")) {
                    if ((<HTMLInputElement>ch).checked) {
                        answer.data.push(ch.parentNode.textContent);
                    }
                }
                this.list.innerHTML = "";
                resolve(answer);
            };
        });
    }
    fill(additive: string[]) {
        for (let text of additive) {
            let li = document.createElement('li');
            let label = document.createElement('label');
            let check = document.createElement('input');
            check.type = "checkbox";
            check.checked = false;
            label.textContent = text;
            label.appendChild(check);
            li.appendChild(label);
            this.list.appendChild(li);
        }
    }
    hide() {
        this.htmlElement.classList.add("hidden");
    }

}
class SequenceAnswerForm implements IAnswerForm {
    htmlElement: HTMLElement;
    form: HTMLFormElement;
    display: HTMLDivElement;
    list: HTMLUListElement;
    answerArray: string[];
    constructor() {
        this.htmlElement = document.getElementById("sequenceAnswerFormWrp");
        this.form = document.querySelector("#sequenceAnswerFormWrp form");
        this.display = document.querySelector("#sequenceAnswerFormWrp div");
        this.list = document.querySelector("#sequenceAnswerFormWrp ul");
        if (!this.htmlElement || !this.form || !this.list || !this.display) {
            throw new ReferenceError("sequenceAnswerForm can't be built, not all of required elements was found");
        }
        this.answerArray = [];
        let cancelBtn = document.querySelector("#sequenceAnswerFormWrp button.cancel") as HTMLButtonElement;
        cancelBtn.onclick = ev => {
            ev.preventDefault();
            this.display.textContent = "";
            this.answerArray = [];
            document.querySelectorAll("#sequenceAnswerFormWrp ul button").forEach(b => (<HTMLButtonElement>b).disabled=false);
        };
    }
    showAndGetAnswer(): Promise<Answer> {
        let answer = new Answer();
        this.htmlElement.classList.remove("hidden");
        return new Promise<Answer>((resolve, reject) => {
            this.form.onsubmit = e => {
                e.preventDefault();
                answer.data = this.answerArray;
                this.answerArray = [];
                this.display.textContent = "";
                this.list.innerHTML = "";
                resolve(answer);
            };
        });
    }
    fill(additive: string[]) {
        for (let text of additive) {
            let btn = document.createElement("button");
            btn.textContent = text;
            btn.onclick = ev => {
                (<HTMLButtonElement>ev.target).disabled = true;
                this.answerArray.push(text);
                if (this.display.textContent.length > 0) this.display.textContent += ", ";
                this.display.textContent += text;
            };

            let li = document.createElement("li");
            li.appendChild(btn);
            this.list.appendChild(li);
        }
    }
    hide() {
        this.htmlElement.classList.add("hidden");
    }

}

class ConnectedPairsAnswerForm implements IAnswerForm{
    htmlElement: HTMLElement;
    form: HTMLFormElement;
    display: HTMLDivElement;
    leftList: HTMLUListElement;
    rightList: HTMLUListElement;
    answerArray: string[];
    constructor() {
        this.htmlElement = document.getElementById("connectedPairsAnswerFormWrp");
        this.display = document.querySelector("#connectedPairsAnswerFormWrp div");
        this.form = document.querySelector("#connectedPairsAnswerFormWrp form");
        this.leftList = document.getElementById("connectedPairsLeftCol") as HTMLUListElement;
        this.rightList = document.getElementById("connectedPairsRightCol") as HTMLUListElement;
        this.answerArray = [];
        let cancelBtn = document.querySelector("#connectedPairsAnswerFormWrp button.cancel") as HTMLButtonElement;
        cancelBtn.onclick = ev => {
            ev.preventDefault();
            this.display.textContent = "";
            this.answerArray = [];
            document.querySelectorAll("#connectedPairsAnswerFormWrp ul button").forEach(b => (<HTMLButtonElement>b).disabled = false);
        };
    }
    showAndGetAnswer(): Promise<Answer> {
        let answer = new Answer();
        this.htmlElement.classList.remove("hidden");
        return new Promise<Answer>((resolve, reject) => {
            this.form.onsubmit = e => {
                e.preventDefault();
                answer.data = this.answerArray;
                this.answerArray = [];
                this.display.textContent = "";
                this.leftList.innerHTML = "";
                this.rightList.innerHTML = "";
                resolve(answer);
            };
        });
    }
    fill(additive: string[]) {
        let counter = 0;
        let boof:HTMLButtonElement[] = [];
        let allButtons:HTMLButtonElement[] = [];
        for (let text of additive) {
            let btn = document.createElement("button");
            btn.textContent = text;
            let li = document.createElement("li");
            li.appendChild(btn);
            allButtons.push(btn);
            if (counter % 2 === 0) {
                btn.onclick = ev => {
                    let senderBtn: HTMLButtonElement = ev.target as HTMLButtonElement;
                    if (boof[1]) {
                        senderBtn.disabled = true;
            			boof[1].classList.remove("choosed");
                        boof[1].disabled = true;
                        if(this.display.textContent.length>0)this.display.textContent+=", ";
                        this.display.textContent += `${senderBtn.textContent} - ${boof[1].textContent}`;
                        this.answerArray.push(senderBtn.textContent);
            			this.answerArray.push(boof[1].textContent);
            			boof.length=0;
            		}else{
                        allButtons.forEach(b => b.classList.remove("choosed"));
                        senderBtn.classList.add("choosed");
                        boof[0] = senderBtn;
            		}
                };
                this.leftList.appendChild(li);
            } else {
                btn.onclick = ev => {
                    let senderBtn: HTMLButtonElement = ev.target as HTMLButtonElement;
                    if (boof[0]) {
                        senderBtn.disabled = true;
            			boof[0].classList.remove("choosed");
                        boof[0].disabled = true;
                        if(this.display.textContent.length>0)this.display.textContent+=", ";
                        this.display.textContent += `${boof[0].textContent} - ${senderBtn.textContent}`;
                        this.answerArray.push(boof[0].textContent);
                        this.answerArray.push(senderBtn.textContent);
            			boof.length=0;
            		}else{
                        allButtons.forEach(b => b.classList.remove("choosed"));
                        senderBtn.classList.add("choosed");
                        boof[1] = senderBtn;
            		}
                };
                this.rightList.appendChild(li);
            }
            counter++;
        }
    }
    hide() {
        this.htmlElement.classList.add("hidden");
    }

}
