class FormManager {
    forms: Map<string, IAnswerForm>;
    constructor() {
        this.forms = new Map<string, IAnswerForm>();
    }
    register(formType: string, form: IAnswerForm) {
        this.forms.set(formType, form);
        form.hide();
    }
    hideForms() {
        for (let item of this.forms) {
            item[1].hide();
        }
    }
    getForm(answerType: string): IAnswerForm {
        return this.forms.get(answerType);
    }
}