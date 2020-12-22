const ratingURI = "http://localhost:50541/api/Reviews"

function addRating(event) {
    const star = event.target.id
    console.log(star)
}

function getRatingForUserAndRecipe(recipeId, userId) {
    fetch(ratingURI + "/" + recipeId + "/" + userId)
        .then(response => response.json())
        .then(data => setRating(data.rating))
        .catch(error => console.error("Error", error))
}

function setRating(rating) {
    starsReversed = document.getElementsByClassName("fa fa-star");
    length = starsReversed.length;
    stars = [];

    for (let index = 0; index < length; index++) {
        stars[index] = starsReversed[length - index - 1];
    }

    while (rating > 0) {
        stars[rating - 1].style.color = "#f3969a";
        rating--;
    }
}