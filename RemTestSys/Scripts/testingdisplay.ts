class TestingDisplay {
    questionNum: HTMLElement;
    questionText: HTMLElement;
    questionSubText: HTMLElement;
    questionCast: HTMLElement;

    constructor(questionNum: HTMLElement, questionText: HTMLElement, questionSubText: HTMLElement, questionCast: HTMLElement) {
        this.questionNum = questionNum;
        this.questionText = questionText;
        this.questionSubText = questionSubText;
        this.questionCast = questionCast;
    }
    update(questionNum: number, questionText: string, questionSubText: string, questionCast: number) {
        this.questionNum.textContent = questionNum.toString();
        this.questionText.textContent = questionText;
        this.questionSubText.textContent = questionSubText;
        this.questionCast.textContent=questionCast;
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