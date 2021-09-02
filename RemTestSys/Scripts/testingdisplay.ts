class TestingDisplay {
    questionNum: HTMLElement;
    questionText: HTMLElement;
    questionSubText: HTMLElement;

    constructor(questionNum: HTMLElement, questionText: HTMLElement, questionSubText: HTMLElement) {
        this.questionNum = questionNum;
        this.questionText = questionText;
        this.questionSubText = questionSubText;
    }
    update(questionNum: number, questionText: string, questionSubText: string) {
        this.questionNum.textContent = questionNum.toString();
        this.questionText.textContent = questionText;
        this.questionSubText.textContent = questionSubText;
    }
    showMessage(text: string, subtext: string) {
        this.questionText.textContent = text;
        this.questionSubText.textContent = subtext;
    }
    clear() {
        this.questionNum.textContent = "";
        this.questionText.textContent = "";
        this.questionSubText.textContent = "";
    }
}