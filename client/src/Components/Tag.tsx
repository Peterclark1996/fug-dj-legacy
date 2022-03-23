import TagData from "../Types/TagData"
import classes from "./Tag.module.scss"

type TagProps = {
    tag: TagData,
}

const Tag = ({ tag }: TagProps) => {
    return (
        <div className={`px-2 me-1 ${classes.rounded} ${classes.shadow} ${classes.smallFont}`} style={{ "backgroundColor": `#${tag.colourHex}` }}>
            {tag.name}
        </div>
    )
}

export default Tag