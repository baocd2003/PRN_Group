document.addEventListener("DOMContentLoaded", function () {
    const numOfLaborsInput = document.querySelector("#Project_NumOfLabors");
    const laborSalaryInput = document.querySelector("#Project_LaborSalaryPerMonth");
    const monthDurationInput = document.querySelector("#Project_MonthDuration");
    const totalPriceInput = document.querySelector("#Project_TotalPrice");

    [numOfLaborsInput, laborSalaryInput, monthDurationInput].forEach(input => {
        input.addEventListener("input", calculateTotalPrice);
    });

    function calculateTotalPrice() {
        const numOfLabors = parseFloat(numOfLaborsInput.value);
        const laborSalary = parseFloat(laborSalaryInput.value);
        const monthDuration = parseFloat(monthDurationInput.value);

        if (!isNaN(numOfLabors) && !isNaN(laborSalary) && !isNaN(monthDuration)) {
            const totalPrice = numOfLabors * laborSalary * monthDuration;
            totalPriceInput.value = totalPrice.toLocaleString();
        } else {
            totalPriceInput.value = "";
        }
    }
});