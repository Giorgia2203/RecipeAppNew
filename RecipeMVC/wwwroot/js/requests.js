const ratingURI = "https://localhost:44331/api/Reviews"
const favouriteURI = "https://localhost:44331/api/FavouriteRecipes"

function addRating(event, userId, recipeId) {
    if (userId) {
        const star = parseInt(event.target.id.split("-").pop())
        review = {
            appUserId: userId,
            recipeId: recipeId,
            rating: star
        };

        fetch(ratingURI + "/" + userId + "/" + recipeId, {
            method: 'PUT',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(review)
        })
            .then(response => response.json())
            .then(() => {
                console.log('THEN')
                getAverageRatingForRecipe(recipeId)
                setRating(star)
            })
            .catch(error => console.error('Unable to add rating.', error));
    }
    else {
        alerta = document.getElementById("warning");
        text = document.getElementById("warningText");
        text.innerHTML = "You have to log in to add a rating to this recipe.";
        alerta.style.display = "block";
        alerta.scrollIntoView();
    }
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

function getAverageRatingForRecipe(recipeId) {
    fetch(ratingURI + "/" + recipeId)
        .then(response => response.json())
        .then(data => setAverage(data))
        .catch(error=> console.error("Error",error))
}

function setAverage(average) {
    averageElement = document.getElementById("average");

    averageElement.innerHTML = average + "/5";
}


function getIsFavourite(userId, recipeId) {
    fetch(favouriteURI + "/" + userId + "/" + recipeId)
        .then(response => response.json())
        .then(data => {
            if (data.status === 404) {
                setFavourite(false)
            } else {
                setFavourite(true)
            }
        })
        .catch(error => {
            setFavourite(false)
            console.error("Error", error)
        })
}

function setFavourite(isFavourite) {
    isFavouriteElement = document.getElementsByClassName("favourite")[0];

    if (isFavourite) {
        isFavouriteElement.style.color = "#e23232"
    } else {
        isFavouriteElement.style.color = "#808080"
    }
}

function favouriteClicked(event,userId,recipeId) {
    heartColor = document.getElementsByClassName("favourite")[0].style.color

    if (userId) {
        if (heartColor === "rgb(226, 50, 50)")
        {
            fetch(favouriteURI + "/" + userId + "/" + recipeId, {
                method: 'DELETE'
            })
                .then(response => response.json())
                .then(() => setFavourite(false))
                .catch(error => console.error('Unable to delete favourite recipe.', error));
        }
        else
        {
            favouriteRecipe =
            {
                appUserId: userId,
                recipeId: recipeId
            };
    
            fetch(favouriteURI, {
                method: 'POST',
                headers: {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(favouriteRecipe)
            })
                .then(response => response.json())
                .then(() => setFavourite(true))
                .catch(error => console.error('Unable to add as favourite recipe.', error));
        }
    }
    else {
        alerta = document.getElementById("warning");
        text = document.getElementById("warningText");
        text.innerHTML = "You have to log in to add this recipe as your favourite.";
        alerta.style.display = "block";
        alerta.scrollIntoView();
    }
}