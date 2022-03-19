class Answer {
}

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
class SomeVariantsAnswerForm {
    constructor() {
        this.htmlElement = document.getElementById("someVariantAnswerFormWrp");
        this.form = document.querySelector("#someVariantAnswerFormWrp form");
        this.list = document.querySelector("#someVariantAnswerFormWrp ul");
        if (!this.htmlElement || !this.form || !this.list) {
            throw new ReferenceError("someVariantForm can't be built, not all of required elements was found");
        }
    }
    showAndGetAnswer() {
        let answer = new Answer();
        answer.data = [];
        this.htmlElement.classList.remove("hidden");
        return new Promise((resolve, reject) => {
            this.form.onsubmit = e => {
                e.preventDefault();
                for (let ch of document.querySelectorAll("#someVariantAnswerFormWrp ul li [type='checkbox']")) {
                    if (ch.checked) {
                        answer.data.push(ch.parentNode.textContent);
                    }
                }
                this.list.innerHTML = "";
                resolve(answer);
            };
        });
    }
    fill(additive) {
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
class SequenceAnswerForm {
    constructor() {
        this.htmlElement = document.getElementById("sequenceAnswerFormWrp");
        this.form = document.querySelector("#sequenceAnswerFormWrp form");
        this.display = document.querySelector("#sequenceAnswerFormWrp div");
        this.list = document.querySelector("#sequenceAnswerFormWrp ul");
        if (!this.htmlElement || !this.form || !this.list || !this.display) {
            throw new ReferenceError("sequenceAnswerForm can't be built, not all of required elements was found");
        }
        this.answerArray = [];
        let cancelBtn = document.querySelector("#sequenceAnswerFormWrp button.cancel");
        cancelBtn.onclick = ev => {
            ev.preventDefault();
            this.display.textContent = "";
            this.answerArray = [];
            document.querySelectorAll("#sequenceAnswerFormWrp ul button").forEach(b => b.disabled = false);
        };
    }
    showAndGetAnswer() {
        let answer = new Answer();
        this.htmlElement.classList.remove("hidden");
        return new Promise((resolve, reject) => {
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
    fill(additive) {
        for (let text of additive) {
            let btn = document.createElement("button");
            btn.textContent = text;
            btn.onclick = ev => {
                ev.target.disabled = true;
                this.answerArray.push(text);
                if (this.display.textContent.length > 0)
                    this.display.textContent += ", ";
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
class ConnectedPairsAnswerForm {
    constructor() {
        this.htmlElement = document.getElementById("connectedPairsAnswerFormWrp");
        this.display = document.querySelector("#connectedPairsAnswerFormWrp div");
        this.form = document.querySelector("#connectedPairsAnswerFormWrp form");
        this.leftList = document.getElementById("connectedPairsLeftCol");
        this.rightList = document.getElementById("connectedPairsRightCol");
        this.answerArray = [];
        let cancelBtn = document.querySelector("#connectedPairsAnswerFormWrp button.cancel");
        cancelBtn.onclick = ev => {
            ev.preventDefault();
            this.display.textContent = "";
            this.answerArray = [];
            document.querySelectorAll("#connectedPairsAnswerFormWrp ul button").forEach(b => b.disabled = false);
        };
    }
    showAndGetAnswer() {
        let answer = new Answer();
        this.htmlElement.classList.remove("hidden");
        return new Promise((resolve, reject) => {
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
    fill(additive) {
        let counter = 0;
        let boof = [];
        let allButtons = [];
        for (let text of additive) {
            let btn = document.createElement("button");
            btn.textContent = text;
            let li = document.createElement("li");
            li.appendChild(btn);
            allButtons.push(btn);
            if (counter % 2 === 0) {
                btn.onclick = ev => {
                    let senderBtn = ev.target;
                    if (boof[1]) {
                        senderBtn.disabled = true;
                        boof[1].classList.remove("choosed");
                        boof[1].disabled = true;
                        if (this.display.textContent.length > 0)
                            this.display.textContent += ", ";
                        this.display.textContent += `${senderBtn.textContent} - ${boof[1].textContent}`;
                        this.answerArray.push(senderBtn.textContent);
                        this.answerArray.push(boof[1].textContent);
                        boof.length = 0;
                    }
                    else {
                        allButtons.forEach(b => b.classList.remove("choosed"));
                        senderBtn.classList.add("choosed");
                        boof[0] = senderBtn;
                    }
                };
                this.leftList.appendChild(li);
            }
            else {
                btn.onclick = ev => {
                    let senderBtn = ev.target;
                    if (boof[0]) {
                        senderBtn.disabled = true;
                        boof[0].classList.remove("choosed");
                        boof[0].disabled = true;
                        if (this.display.textContent.length > 0)
                            this.display.textContent += ", ";
                        this.display.textContent += `${boof[0].textContent} - ${senderBtn.textContent}`;
                        this.answerArray.push(boof[0].textContent);
                        this.answerArray.push(senderBtn.textContent);
                        boof.length = 0;
                    }
                    else {
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

class FormManager {
    constructor() {
        this.forms = new Map();
    }
    register(formType, form) {
        this.forms.set(formType, form);
        form.hide();
    }
    hideForms() {
        for (let item of this.forms) {
            item[1].hide();
        }
    }
    getForm(answerType) {
        return this.forms.get(answerType);
    }
}

class ResultOfAnswer {
}

var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
class Server {
    constructor(url, sessionId) {
        this.url = url;
        this.sessionId = sessionId;
        this.testState = null;
    }
    answer(answer) {
        return __awaiter(this, void 0, void 0, function* () {
            return yield this._postRequest(answer);
        });
    }
    updateState() {
        return __awaiter(this, void 0, void 0, function* () {
            let resp = yield fetch(`${this.url}/${this.sessionId}`);
            if (resp.ok) {
                this.testState = yield resp.json();
            }
            else {
                throw new Error(resp.statusText);
            }
        });
    }
    _postRequest(dataObj) {
        return __awaiter(this, void 0, void 0, function* () {
            let resp = yield fetch(`${this.url}/${this.sessionId}`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(dataObj)
            });
            if (resp.ok) {
                return yield resp.json();
            }
            else {
                throw new Error(resp.statusText);
            }
        });
    }
}

var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
window.addEventListener("load", function () {
    return __awaiter(this, void 0, void 0, function* () {
        let display = new TestingDisplay(document.getElementById("questionNum"), document.getElementById("questionText"), document.getElementById("questionSubText"), document.getElementById("questionCast"));
        let formManager = new FormManager();
        formManager.register("confirm", new ConfirmForm());
        let textAnswerForm = new TextAnswerForm();
        formManager.register("TextAnswer", textAnswerForm);
        formManager.register("OneOfFourVariantsAnswer", new OneOfFourVariantsAnswerForm());
        formManager.register("SomeVariantsAnswer", new SomeVariantsAnswerForm());
        formManager.register("SequenceAnswer", new SequenceAnswerForm());
        formManager.register("ConnectedPairsAnswer", new ConnectedPairsAnswerForm());
        formManager.hideForms();
        let confirmForm = formManager.getForm("confirm");
        let timer = new TestingTimer(document.getElementById("timerDisp"));
        let sessionId = document.getElementById("scriptData").dataset.sessionid;
        let server = new Server("/api/Testing", Number.parseInt(sessionId));
        yield server.updateState();
        timer.time = server.testState.timeLeft;
        timer.start();
        while (!server.testState.finished) {
            display.update(server.testState.questionNum, server.testState.questionText, server.testState.questionSubText, server.testState.questionCost);
            let aForm = formManager.getForm(server.testState.answerType);
            aForm.fill(server.testState.addition);
            let answer = yield aForm.showAndGetAnswer();
            aForm.hide();
            let answerResult = yield server.answer(answer);
            if (answerResult.isRight) {
                display.showMessage("Правильно!", "");
            }
            else {
                display.showMessage("Неправильно!", `Правильна відповідь:\n${answerResult.rightText}`);
            }
            yield confirmForm.showAndGetAnswer();
            confirmForm.hide();
            display.clear();
            yield server.updateState();
            timer.time = server.testState.timeLeft;
        }
        window.location.href = `${document.getElementById("scriptData").dataset.hrefresult}/${server.testState.resultId}`;
    });
});

class TestingDisplay {
    constructor(questionNum, questionText, questionSubText, questionCast) {
        this.questionNum = questionNum;
        this.questionText = questionText;
        this.questionSubText = questionSubText;
        this.questionCast = questionCast;
    }
    update(questionNum, questionText, questionSubText, questionCast) {
        this.questionNum.textContent = questionNum.toString();
        this.questionText.textContent = questionText;
        this.questionSubText.textContent = questionSubText;
        this.questionCast.textContent = questionCast.toString();
    }
    showMessage(text, subtext) {
        this.questionText.textContent = text;
        this.questionSubText.textContent = subtext;
    }
    clear() {
        this.questionNum.textContent = "";
        this.questionText.textContent = "";
        this.questionSubText.textContent = "";
    }
}

class TestingTimer {
    constructor(dispElement) {
        this.dispElement = dispElement;
        this.timeLeft = 0;
    }
    get time() {
        return this.timeLeft;
    }
    set time(val) {
        this.timeLeft = val < 0 ? 0 : val;
        this._updateDisp();
    }
    get finished() {
        return this.timeLeft <= 0;
    }
    start() {
        setTimeout(() => this.tick(), 1000);
    }
    tick() {
        this.time -= 1;
        this._updateDisp();
        if (this.finished)
            return;
        setTimeout(() => this.tick(), 1000);
    }
    _updateDisp() {
        let min = Math.floor(this.timeLeft / 60);
        let sec = this.timeLeft % 60;
        this.dispElement.textContent = `${min < 10 ? '0' : ''}${min} : ${sec < 10 ? '0' : ''}${sec}`;
    }
}

class TestState {
}
