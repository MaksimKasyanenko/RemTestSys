class FormManager {
    formContainer: HTMLElement;
    forms: Map<string, IAnswerForm>;
    constructor(formContainer: HTMLElement) {
        this.formContainer = formContainer;
        this.forms = new Map<string, IAnswerForm>();
    }
    register(formType: string, form: IAnswerForm) {
        this.forms.set(formType, form);
        form.hide();
        this.formContainer.append(form.htmlElement);
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