class FormManager {
    constructor(formContainer) {
        this.formContainer = formContainer;
        this.forms = new Map();
    }
    register(formType, form) {
        this.forms.set(formType, form);
        form.hide();
        this.formContainer.append(form.htmlElement);
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
