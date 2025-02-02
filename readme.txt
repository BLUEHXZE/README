# Push changes
git push
git push origin main (alleen als git push niet werkt)

# Pull the latest changes (without rebase)
git pull origin main  

# Delete all uncommitted changes (WARNING: This cannot be undone)
git reset --hard HEAD  

# Discard changes in a specific file (optional)
git checkout -- filename.ext  

# Refresh (fetch latest changes without merging)
git fetch origin main  

# Check status and logs after fetching
git status  
git log --oneline origin/main  

 
