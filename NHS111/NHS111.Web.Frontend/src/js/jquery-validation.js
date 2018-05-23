
jQuery.validator.setDefaults({
    ignore: "[type='hidden']",
    focusInvalid: false,
    showErrors: function (errorMap, errorList) {
        // This is a modified version of validate.unobtrusive's default error summary
        // it adds links to the error fields
        var self = this
        var container = $(this.currentForm).find("[data-valmsg-summary=true]")
        if (!container.length) return this.defaultShowErrors()

        var list = container.find(".js-error-list-original").hide(),
            elements = this.elements().toArray(),
            newList = container.find(".js-error-list").empty().show(),
            invalid = self.invalid

        container.hide()

        container.removeAttr("role")
            
        elements.forEach((val) => {
            var containerID = val.id
            var title = ($(val).attr('type') == 'radio' || $(val).attr('type') == 'checkbox') ? $(val).parents("fieldset").children("legend")[0].innerText : $(val).siblings("label")[0].innerText
            if (this.invalid[val.name]) $("<li />").html(`<a href="#${containerID}">${title}</a>`).appendTo(newList)
        }, this)
        
        if (!Object.keys(invalid).length) { // if valid, hide error summary 
            container.addClass("validation-summary-valid").removeClass("validation-summary-errors")
        }
        else { // if not valid, add class (which is then used to trigger focus and alert screenreaders)
            container.addClass("validation-summary-errors").removeClass("validation-summary-valid")
        }
        this.defaultShowErrors()
    },
    highlight: function (element, errorClass, validClass) {
        $(element).closest(".form-group").addClass("form-group-error")
        $(element).siblings(".error-message").attr("role", "alert")
        $(element).attr("aria-invalid", "true")
        $(`[name="${$(element).attr("name")}"]`).attr("aria-invalid", "true")
    },
    unhighlight: function (element, errorClass, validClass) {
        $(element).closest(".form-group").removeClass("form-group-error")
        $(element).siblings(".error-message").removeAttr("role")
        $(element).removeAttr("aria-invalid")
        $(`[name="${$(element).attr("name")}"]`).removeAttr("aria-invalid")
    }
})

jQuery(document).ready(function () {
    $("main form").on("submit", function (e) {
        const container = $(".validation-summary-errors")
        $("[role='alert']").removeAttr("role")
        container.show()
        if (container.length) { // if it isn't valid, make sure screenreaders get alerted to the box
            setTimeout(() => {
                $(".js-error-list li:first-child a").focus()
                container.attr("role", "alert").attr("aria-live", "assertive")
                $("[role='alert']").removeAttr("role").removeAttr("aria-live")
            }, 100)
        }
    })
})
