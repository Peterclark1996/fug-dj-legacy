import classes from "./LinkButton.module.scss"

type LinkButtonProps = {
    linkUrl: string,
    text: string,
}

const LinkButton = ({ linkUrl, text }: LinkButtonProps) => {
    return (
        <a className={`px-1 rounded bg-danger ${classes.mediumFont} ${classes.noLinkStyle}`} href={linkUrl} target="_blank" rel="noopener noreferrer">{text}</a>
    )
}

export default LinkButton