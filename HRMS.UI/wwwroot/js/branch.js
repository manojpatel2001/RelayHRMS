function displayBranch() {
    // Hide the help div
    var helpDiv = document.getElementById('hideDiv');
    if (helpDiv) {
        helpDiv.style.display = 'none'; // Hide the help div
    }

    // Show the branch form div
    var branchForm = document.getElementById('branchForm');
    if (branchForm) {
        branchForm.style.display = 'block'; // Show the branch form
    }
}
